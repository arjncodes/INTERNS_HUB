using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace INTERNS_HUB.Models
{
    public class CheckBoxListHelper
    {
        public string Value { get; set; }
        public string Text { get; set; }
        public bool IsChecked { get; set; }

    }
    public class CandidateProfile
    {
       
        [Required(ErrorMessage = "Age is required.")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Location is required.")]
        public string Place { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [Phone (ErrorMessage ="enter valid mobile number")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Highest Qualification is required.")]
        public string HighestQualification { get; set; }

        [Required(ErrorMessage = "Year of Completion is required.")]
        public DateTime YearOfCompletion { get; set; }

        [Required(ErrorMessage = "Experience is required.")]
        public string Experiance { get; set; }

        [Required(ErrorMessage = "Resume is required.")]
        public HttpPostedFileBase ResumeFile { get; set; }

        public List<Skill> Skills { get; set; }
        public string[] SelectedSkills { get; set; }
    }
}