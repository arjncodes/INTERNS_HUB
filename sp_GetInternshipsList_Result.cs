//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace INTERNS_HUB
{
    using System;
    
    public partial class sp_GetInternshipsList_Result
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public string Type { get; set; }
        public string SkillsRequired { get; set; }
        public System.DateTime StartDate { get; set; }
        public string Duration { get; set; }
        public decimal Stipend { get; set; }
        public int Openings { get; set; }
        public System.DateTime Deadline { get; set; }
        public string CompanyName { get; set; }
        public string ContactEmail { get; set; }
        public Nullable<int> CompanyId { get; set; }
    }
}