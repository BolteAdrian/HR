using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HR.Models
{
    public partial class Interviews
    {
        public Interviews()
        {
            InterviewTeam = new HashSet<InterviewTeams>();
        }

        public long Id { get; set; }
        public long CandidateId { get; set; }
        public DateTime? InterviewDate { get; set; }
        
        public long? FunctionApply { get; set; }
        public long? DepartamentApply { get; set; }
        public bool? Accepted { get; set; }
        [Required]
        public string TestResult { get; set; }
        public int? RefusedReason { get; set; }
        public string RefusedObservation { get; set; }
        public string Comments { get; set; }
    
        public DateTime? DateAnswer { get; set; }
        public int? OfferStatus { get; set; }
        public DateTime? EmploymentDate { get; set; }
        public string AddedBy { get; set; }
        public DateTime? AddedAt { get; set; }
        public string UpdatedBy { get; set; }
        public string UpdatedAt { get; set; }

        public virtual Candidates Candidate { get; set; }
        public virtual ICollection<InterviewTeams> InterviewTeam { get; set; }
    }
}
