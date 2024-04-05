using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace INTERNS_HUB.Models
{
    public class CompanyLogin
    {
        [Required(ErrorMessage ="Email is needed")]
        [EmailAddress(ErrorMessage ="enter valid email")]

        public string CompanyEmail { get; set; }
        [Required(ErrorMessage ="password is needed")]  

        public string CompanyPassword { get; set; }
    }
}