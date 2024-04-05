using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using INTERNS_HUB.Models;

namespace INTERNS_HUB.Controllers
{
    public class NormalUserController : Controller
    {
        INTERNS_HUBEntities dbobj = new INTERNS_HUBEntities();
        // GET: NormalUser
        
        public ActionResult UserRegistration_PageLoad()
        {
            return View();
        }

        public ActionResult UserRegistration_BtnClick(NormalUserReg clsobj)
        {
            if (ModelState.IsValid)
            {
                var HashedPassword = BCrypt.Net.BCrypt.HashPassword(clsobj.LoginPassword);

                var GetMaxId = dbobj.sp_GetMaxId().FirstOrDefault(); // Assuming GetMaxId is 2
                int maxId = GetMaxId != null ? GetMaxId.Value : 0; // maxId will be 2
                int regId = maxId + 1;

                var result = dbobj.sp_UserRegister(clsobj.Email, clsobj.FullName, HashedPassword, regId);

                if (result >0)
                {
                    return RedirectToAction("CandidateLogin_PageLoad");
                }
                else
                {
                    ModelState.AddModelError("", "Registration failed. Please try again.");
                }
            }

            return View("UserRegistration_PageLoad", clsobj);
        }

        public ActionResult CandidateLogin_PageLoad()
        {
            return View();
        }

        public ActionResult CandidateLogin_BtnClick(UserLoginCls clsobj)
        {
            if (ModelState.IsValid)
            {
                var user = dbobj.UserLoginCredentials.FirstOrDefault(u => u.Email == clsobj.Email);
                if (user != null && BCrypt.Net.BCrypt.Verify(clsobj.LoginPassword, user.LoginPassword))
                {
                    Session["UserId"] = user.UserId;
                    Session["FullName"] = user.FullName;

                    return RedirectToAction("UserDashboard_PageLoad", "NormalUser");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid login attempt");
                }
            }
            // If ModelState is not valid, return the view with a new NormalUserReg object to display validation errors
            return View("CandidateLogin_PageLoad", clsobj);
        }

        public ActionResult UserDashboard_PageLoad()
        {
            if(Session["UserId"] !=null && Session["FullName"] != null)
            {
                var fullName = Session["FullName"].ToString();
                ViewBag.FullName = fullName;
                return View();
            }
            else
            {
                return RedirectToAction("CandidateLogin_PageLoad", "NormalUser");
            }
            
        }

        public ActionResult InternshipsList_PageLoad()
        {
            var postedJobs = dbobj.sp_GetJobs().ToList();
            return View(postedJobs);
        }
            
        [HttpPost]
        public ActionResult UserApplyingJob_BtnClick(int jobId)
        {
            if(Session["UserId"] != null)
            {
                int userId = (int)Session["UserId"];
                bool alreadyRegistered = dbobj.CompanyApplication.Any(ca => ca.UserId == userId && ca.InternshipId == jobId);

                if (alreadyRegistered)
                {
                    ModelState.AddModelError("", "You have already applied for this job.");
                    return View("UserDashboard_PageLoad");
                }

                var jobDetails = dbobj.Internships.FirstOrDefault(i => i.Id == jobId);
                if(jobDetails != null)
                {
                    CompanyApplication application = new CompanyApplication
                    {
                        UserId = userId,
                        InternshipId = jobId,
                        AppliedDate = DateTime.Now,
                        CompanyId = (int)jobDetails.CompanyId

                    };
                    dbobj.CompanyApplication.Add(application);
                    dbobj.SaveChanges();
                    TempData["Message"] = "Application submitted successfully.";
                    return RedirectToAction("InternshipsList_PageLoad");

                }
            }
            return View();
        }

        // load update page
        public ActionResult UserProfile_PageLoad()
        {
            int userId = (int)Session["UserId"];
            List<UserSkill> userSkills = dbobj.UserSkills.Where(us => us.UserId == userId).ToList();
            List<Skill> allSkills = dbobj.Skills.ToList();

            // objects new instance
            CandidateProfile userProfile = new CandidateProfile();
            // to the list in the property

            userProfile.Skills = allSkills;

            userProfile.SelectedSkills = dbobj.UserSkills.Where(us => us.UserId == userId).Select(us => us.SkillsId.ToString()).ToArray();

            return View(userProfile);
        }

        public ActionResult UserProfile_BtnClick(CandidateProfile clsobj)
        {
            int userId = (int)Session["UserId"];
            if (ModelState.IsValid)
            {
                string filePath = SaveFile(clsobj.ResumeFile);
                var result = dbobj.sp_UpdateProfile(userId, clsobj.Age, clsobj.Gender, clsobj.Place, clsobj.Phone, clsobj.HighestQualification, clsobj.YearOfCompletion, clsobj.Experiance, filePath);
                if(result != 0)
                {
                    return RedirectToAction("UserProfile_PageLoad");
                }
                else
                {
                    return RedirectToAction("UserDashboard_PageLoad");
                }
            }
            return View();
        }

        private string SaveFile(HttpPostedFileBase resumeFile)
        {
            if(resumeFile != null && resumeFile.ContentLength>0)
            {
                string fileName = Path.GetFileNameWithoutExtension(resumeFile.FileName) + "_" + Guid.NewGuid().ToString() + Path.GetExtension(resumeFile.FileName);
                string filePath = Path.Combine(Server.MapPath("~/UploadedFiles"), fileName);

                resumeFile.SaveAs(filePath);
                return filePath;
            }
            return null;

        }

        [HttpPost]
        public ActionResult RemoveSkill(int skillId)
        {
            
            return RedirectToAction("UserProfile_PageLoad");
        }

        public ActionResult ChangePassword_PageLoad()
        {
            return View();
        }

        public ActionResult ChangePassword_BtnClick(ChangePasswordViewModel clsobj)
        {
            if (ModelState.IsValid)
            {
                int userId = (int)Session["UserId"];
                var user = dbobj.UserLoginCredentials.FirstOrDefault(u => u.UserId == userId);
                if(user != null && BCrypt.Net.BCrypt.Verify(clsobj.OldPassword, user.LoginPassword))
                {
                    user.LoginPassword = BCrypt.Net.BCrypt.HashPassword(clsobj.NewPassword);
                    dbobj.SaveChanges();
                    TempData["SuccessMessage"] = "Password Changed Successfully";
                    return RedirectToAction("UserDashboard_PageLoad");

                }
                ModelState.AddModelError("", "Invalid old password.");
                return View("ChangePassword_PageLoad", clsobj);
            }
            return View();
        }

        public ActionResult UserLogout_BtnClick()
        {
            Session.Clear();
            Session.Abandon();

            return RedirectToAction("CandidateLogin_PageLoad");
        }
    }
}