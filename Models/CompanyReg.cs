using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace INTERNS_HUB.Models
{
    public class CompanyReg
    {
        [Required(ErrorMessage = "Company Name is required")]
        public string CompanyName { get; set; }

        [Required (ErrorMessage ="Company description is needed")]
        public string CompanyDescription { get; set; }

        [Required(ErrorMessage = "Company Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string CompanyEmail { get; set; }

        [Required(ErrorMessage = "Company Password is required")]
        public string CompanyPassword { get; set; }

        [Required (ErrorMessage ="Confirm your password")]
        [Compare("CompanyPassword", ErrorMessage ="password mismatch")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Company Location is required")]
        public string CompanyLocation { get; set; }

        public string Industry { get; set; }

        public string Website { get; set; }

    }
}