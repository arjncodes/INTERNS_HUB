using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace INTERNS_HUB.Models
{
    public class NormalUserReg
    {
        [Key]
        [Required(ErrorMessage = "email required")]
        [EmailAddress(ErrorMessage = "Invalid email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "name required")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Password required")]
        public string LoginPassword { get; set; } 

        [Required(ErrorMessage = "confirm password required")]
        [Compare("LoginPassword", ErrorMessage = "Password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

}