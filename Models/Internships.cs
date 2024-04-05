using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace INTERNS_HUB.Models
{
   
    public class Internships
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please enter a job title")]
        
        public string Title { get; set; }

        [Required(ErrorMessage = "Please enter a job description")]
        
        public string Description { get; set; }

        [Required(ErrorMessage = "Please enter a job location")]
        
        public string Location { get; set; }

        [Required(ErrorMessage = "Please select a job type")]
        public string Type { get; set; }

        [Required(ErrorMessage = "Please enter skills required")]
        public string SkillsRequired { get; set; }

        [Required(ErrorMessage = "Please enter a start date")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "Please enter duration")]
        public string Duration { get; set; }

        [Required(ErrorMessage = "Please enter stipend")]
        public decimal Stipend { get; set; }

        [Required(ErrorMessage = "Please enter number of openings")]
        public int Openings { get; set; }

        [Required(ErrorMessage = "Please enter an application deadline")]
        public DateTime Deadline { get; set; }

        
        [Required(ErrorMessage = "Please enter a contact email")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string ContactEmail { get; set; }
    }
}