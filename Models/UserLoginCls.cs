using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace INTERNS_HUB.Models
{
    public class UserLoginCls
    {
        [Required(ErrorMessage = "email required")]
        [EmailAddress(ErrorMessage = "Invalid email")]

        public string Email { get; set; }

        [Required(ErrorMessage = "Password required")]
        public string LoginPassword { get; set; }
    }
}