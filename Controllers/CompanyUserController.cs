using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using INTERNS_HUB.Models;

namespace INTERNS_HUB.Controllers
{
    public class CompanyUserController : Controller
    {
        INTERNS_HUBEntities dbobj = new INTERNS_HUBEntities();
        // GET: CompanyUser

        public ActionResult CompanyRegistration_PageLoad()
        {
            return View();
        }

        public ActionResult CompanyRegistration_BtnClick(CompanyReg clsobj)
        {
            if (ModelState.IsValid)
            {
                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(clsobj.CompanyPassword);

                var result = dbobj.sp_RegisterCompany(clsobj.CompanyName, clsobj.CompanyDescription, clsobj.CompanyLocation, clsobj.Industry, clsobj.Website, clsobj.CompanyEmail, hashedPassword);
                if (result > 0)
                {
                    return RedirectToAction("CompanyLogin_PageLoad");
                }
                else
                {
                    ModelState.AddModelError("", "Registration failed. Please try again.");
                }

            }
            return View("CompanyRegistration_PageLoad", clsobj);
        }

        public ActionResult CompanyLogin_PageLoad()
        {
            return View();
        }

        public ActionResult CompanyLogin_BtnClick(CompanyLogin clsobj)
        {
            if (ModelState.IsValid)
            {
                var user = dbobj.CompanyRegistrations.FirstOrDefault(u => u.CompanyEmail == clsobj.CompanyEmail);
                if (user != null && BCrypt.Net.BCrypt.Verify(clsobj.CompanyPassword, user.CompanyPassword))
                {
                    Session["CompanyId"] = user.CompanyId;
                    Session["CompanyName"] = user.CompanyName;
                    return RedirectToAction("CompanyDashboard_PageLoad", "CompanyUser");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid login attempt");
                }
            }
            return View("CompanyLogin_PageLoad", clsobj);
        }

        public ActionResult CompanyDashboard_PageLoad()
        {
            if (Session["CompanyId"] != null && Session["CompanyName"] != null)
            {
                var companyName = Session["CompanyName"].ToString();
                int companyId = (int)Session["CompanyId"];
                
                ViewBag.CompanyName = companyName;

                int countOfPostedJobs = dbobj.Internships.Count(p => p.CompanyId == companyId);
                ViewBag.CountOfPostedJobs = countOfPostedJobs;
                var query = $"SELECT COUNT(*) FROM CompanyApplication WHERE CompanyId = {companyId}";
                int countOfCandidates = dbobj.Database.SqlQuery<int>(query).Single();
                ViewBag.CountOfCandidates = countOfCandidates;


                return View();
            }
            else
            {
                return RedirectToAction("CompanyLogin_PageLoad", "CompanyUser");
            }
        }

        public ActionResult PostInternships_PageLoad()
        {
            if (Session["CompanyId"] != null && Session["CompanyName"] != null)
            {
                var companyName = Session["CompanyName"].ToString();
                ViewBag.CompanyName = companyName;
                return View();
            }
            else
            {
                return RedirectToAction("CompanyLogin_PageLoad", "CompanyUser");
            }
        }

        [HttpPost]
        public ActionResult PostInternship_BtnClick(Internships clsobj)
        {
            var companyName = Session["CompanyName"].ToString();
            int companyId = Convert.ToInt32(Session["CompanyId"]);

            if (ModelState.IsValid)
            {
                if (clsobj.Id == 0)
                {
                    // Posting a new internship
                    var result = dbobj.sp_PostInternships(clsobj.Title, clsobj.Description, clsobj.Location, clsobj.Type, clsobj.SkillsRequired, clsobj.StartDate, clsobj.Duration, clsobj.Stipend, clsobj.Openings, clsobj.Deadline, companyName, clsobj.ContactEmail, companyId);
                    if (result != 0)
                    {
                        return RedirectToAction("PostInternships_PageLoad");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Failed to post internship. Please try again.");
                    }
                }
                else
                {
                    // Editing an existing internship
                    var existingJob = dbobj.Internships.FirstOrDefault(j => j.Id == clsobj.Id);
                    if (existingJob != null)
                    {
                        existingJob.Title = clsobj.Title;
                        existingJob.Description = clsobj.Description;
                        existingJob.Location = clsobj.Location;
                        existingJob.Type = clsobj.Type;
                        existingJob.SkillsRequired = clsobj.SkillsRequired;
                        existingJob.StartDate = clsobj.StartDate;
                        existingJob.Duration = clsobj.Duration;
                        existingJob.Stipend = clsobj.Stipend;
                        existingJob.Openings = clsobj.Openings;
                        existingJob.Deadline = clsobj.Deadline;
                        existingJob.ContactEmail = clsobj.ContactEmail;

                        dbobj.SaveChanges();

                        return RedirectToAction("PostedJobList_PageLoad");
                    }
                    else
                    {
                        return HttpNotFound();
                    }
                }
            }

            // If model state is not valid, return to the same view with the model
            return View("PostedInternshipsView", clsobj);
        }


        // to list the jobs

        public ActionResult PostedJobList_PageLoad()
        {
            PostedInternshipsViewModel viewModel = new PostedInternshipsViewModel();

            if (Session["CompanyName"] != null)
            {
                var companyName = Session["CompanyName"].ToString();
                ViewBag.CompanyName = companyName;

                if (Session["CompanyId"] != null && int.TryParse(Session["CompanyId"].ToString(), out int companyId))
                {
                    var spInternships = dbobj.sp_GetInternshipsList(companyId).ToList();
                    viewModel.PostedJobs = spInternships.Select(sp => new PostedInternshipsViewModel
                    {
                        Id=sp.Id,
                        Title = sp.Title,
                        Description = sp.Description,
                        CompanyName = sp.CompanyName,
                        Location = sp.Location,
                        Type = sp.Type,
                        SkillsRequired = sp.SkillsRequired,
                        StartDate = sp.StartDate,
                        Duration = sp.Duration,
                        Stipend = sp.Stipend,
                        Openings = sp.Openings,
                        Deadline = sp.Deadline,
                        ContactEmail = sp.ContactEmail
                    }).ToList();
                }
            }
            else
            {
                return RedirectToAction("CompanyLogin_PageLoad", "CompanyUser");
            }


            return View(viewModel);
        }

        public ActionResult EditInternship_PageLoad(int? id)
        {
            if (!id.HasValue)
            {

                return HttpNotFound();
            }

            // Get the internship details from the database
            var internship = GetInternshipById(id);

            if (internship == null)
            {
                return HttpNotFound();
            }

            // Map the internship details to a view model
            var viewModel = new PostedInternshipsViewModel
            {
                Id = internship.Id,
                Title = internship.Title,
                Description = internship.Description,
                Location = internship.Location,
                Type = internship.Type,
                SkillsRequired = internship.SkillsRequired,
                StartDate = internship.StartDate,
                Duration = internship.Duration,
                Stipend = internship.Stipend,
                Openings = internship.Openings,
                Deadline = internship.Deadline,
                ContactEmail = internship.ContactEmail
            };

            return View(viewModel);
        }

        private Internship GetInternshipById(int? id)
        {
            return dbobj.Internships.FirstOrDefault(i => i.Id == id);
        }

        [HttpPost]
        public ActionResult EditInternships_BtnClick(PostedInternshipsViewModel clsobj)
        {
            if (ModelState.IsValid)
            {

                // Update the internship details in the database
                var existingJob = dbobj.Internships.FirstOrDefault(j => j.Id == clsobj.Id);
                if (existingJob != null)
                {
                    existingJob.Title = clsobj.Title;
                    existingJob.Description = clsobj.Description;
                    existingJob.Location = clsobj.Location;
                    existingJob.Type = clsobj.Type;
                    existingJob.SkillsRequired = clsobj.SkillsRequired;
                    existingJob.StartDate = clsobj.StartDate;
                    existingJob.Duration = clsobj.Duration;
                    existingJob.Stipend = clsobj.Stipend;
                    existingJob.Openings = clsobj.Openings;
                    existingJob.Deadline = clsobj.Deadline;
                    existingJob.ContactEmail = clsobj.ContactEmail;

                    dbobj.SaveChanges();

                    return RedirectToAction("PostedJobList_PageLoad");
                }
                else
                {
                    return HttpNotFound();
                }
            }

            // If model state is not valid, return to the same view with the model
            return View(clsobj);
        }

        public ActionResult ViewAppliedJobs()
        {
            if (Session["CompanyId"] != null && Session["CompanyName"]!= null)
            {
                int companyId = (int)Session["CompanyId"];
                var companyName = Session["CompanyName"].ToString();
                ViewBag.CompanyName = companyName;
                var details = dbobj.sp_GetApplicants(companyId).Select(a => new ApplicantsModel
                {
                    UserId = a.UserId,
                    FullName = a.FullName,
                    Email = a.UserEmail,
                    Age = a.Age.HasValue ? (int)a.Age : 0, // Use a default value if Age is null
                    Gender = a.Gender,
                    Place = a.Place,
                    Phone = a.Phone,
                    HighestQualification = a.HighestQualification,
                    YearOfCompletion = a.YearOfCompletion,
                    Experiance = a.Experiance,
                    ResumeFile = a.ResumeFile
                }).ToList();

                return View(details);
            }
            return RedirectToAction("CompanyDashboard_PageLoad", "CompanyUser");
        }

        public ActionResult UserDetailsProfile(int userId)
        {
            var user = dbobj.UserProfile.FirstOrDefault(u => u.UserId == userId);
            if (user == null)
            {
                return HttpNotFound();
            }

            return View(user); // Return UserProfile instead of CandidateProfile
        }


        public FileResult DownloadResume(string resumeFile)
        {
            string filePath = Path.Combine(Server.MapPath("~/UploadedFiles/"), resumeFile);

            string contentType = "application/pdf"; // Set the appropriate content type for your file
            string fileName = Path.GetFileName(filePath);
            return File(filePath, contentType, fileName);
        }

        public ActionResult ChangePassword_PageLoad()
        {
            return View();
        }

        public ActionResult ChangePassword_BtnClick(ChangePasswordViewModel clsobj)
        {
            if (ModelState.IsValid)
            {
                if(Session["CompanyId"]!= null)
                {
                    int companyId = (int)Session["CompanyId"];
                    var user = dbobj.CompanyRegistrations.FirstOrDefault(c => c.CompanyId == companyId);
                    if(user != null)
                    {
                        if(BCrypt.Net.BCrypt.Verify(clsobj.OldPassword, user.CompanyPassword))
                        {
                            user.CompanyPassword = BCrypt.Net.BCrypt.HashPassword(clsobj.NewPassword);
                            dbobj.SaveChanges();
                            TempData["SuccessMessage"] = "Password changed successfully.";
                            return RedirectToAction("CompanyProfile_PageLoad");
                        }
                        else
                        {
                            ModelState.AddModelError("", "Invalid old password.");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "User not found.");
                    }

                }
                else
                {
                    ModelState.AddModelError("", "Session expired. Please login again.");
                }
                
            }
            return View();
        }
        public ActionResult Logout()
        {
            Session.Clear(); // Clear all session variables
            Session.Abandon(); // Abandon the session
            return RedirectToAction("CompanyLogin_PageLoad", "CompanyUser"); // Redirect to the home page or any other page after logout
        }

        public ActionResult ViewApplications_PageLoad()
        {
            // Retrieve the list of applied candidates from the database
            var appliedCandidates = dbobj.UserProfile.ToList();

            return View(appliedCandidates);
        }

        

    }
}