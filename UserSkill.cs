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
    using System.Collections.Generic;
    
    public partial class UserSkill
    {
        public int UserSkillsId { get; set; }
        public int UserId { get; set; }
        public int SkillsId { get; set; }
    
        public virtual Skill Skill { get; set; }
        public virtual UserLoginCredential UserLoginCredential { get; set; }
    }
}
