using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace INTERNS_HUB.Models
{
    public class CompanyApplicationViewModel
    {
        public int UserId { get; set; }
        public int InternshipsId { get; set; }
        public DateTime AppliedDate { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public string Place { get; set; }
        public string Phone { get; set; }
        public string HighestQualification { get; set; }
        public string YearOfCompletion { get; set; }
        public string Experiance { get; set; }
        public string ResumeFile { get; set; }
        public string InternshipName { get; set; }
    }
}