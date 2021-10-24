using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace HR.Models
{
    public partial class InterviewCv
    {
        public InterviewCv()
        {
            InterviewTeam = new HashSet<InterviewTeam>();
        }

        public long Id { get; set; }
       
        public long PersonCvid { get; set; }
        public DateTime? InterviewDate { get; set; }
        [Required]
        public string FunctionApply { get; set; }
        public string DepartamentApply { get; set; }
        public bool? Accepted { get; set; }
        public string TestResult { get; set; }
        public int? RefusedReason { get; set; }
        public string RefusedObservation { get; set; }
        public string Comments { get; set; }
        public DateTime? DateAnswer { get; set; }
        [Required]
        public int? OffertStatus { get; set; }
        public DateTime? EmploymentDate { get; set; }
        public string AddedBy { get; set; }
        public DateTime? AddedAt { get; set; }
        public string UpdatedBy { get; set; }
        public string UpdatedAt { get; set; }

        public virtual PersonCv PersonCv { get; set; }
        public virtual ICollection<InterviewTeam> InterviewTeam { get; set; }
    }
}
