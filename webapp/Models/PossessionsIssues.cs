//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace VPS.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class PossessionsIssues
    {
        public int PossessionIssueID { get; set; }
        public int PossessionID { get; set; }
        public string IssueDetails { get; set; }
        public bool IsPossessionTimeIssue { get; set; }
        public bool IsReturnTimeIssue { get; set; }
        public string ImagePath { get; set; }
        public string ImagePathThumbnail { get; set; }
    
        public virtual Possessions Possessions { get; set; }
    }
}
