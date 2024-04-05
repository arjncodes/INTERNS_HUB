using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace INTERNS_HUB.Models
{
    public class PostedInternshipsViewModel
    {
        public PostedInternshipsViewModel()
        {
            PostedJobs = new List<PostedInternshipsViewModel>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public string Type { get; set; }
        public string SkillsRequired { get; set; }
        public DateTime StartDate { get; set; }
        public string Duration { get; set; }
        public decimal Stipend { get; set; }
        public int Openings { get; set; }
        public DateTime Deadline { get; set; }
        public string CompanyName { get; set; }
        public string ContactEmail { get; set; }
        public List<PostedInternshipsViewModel> PostedJobs { get; set; }

    }

}