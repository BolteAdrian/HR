using System;

namespace HR.DataModels
{
    public partial class JobApplicationDetails
    {
        public long Id { get; set; }
        public long CandidateId { get; set; }
        public string CandidateName { get; set; }
        public DateTime InterviewDate { get; set; }
        public string Function { get; set; }
        public string Department { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public bool? Accepted { get; set; }
        public string TestResult { get; set; }
        public int? RefusedReason { get; set; }
        public string Comments { get; set; }
        public DateTime? DateAnswer { get; set; }
        public int? OfferStatus { get; set; }
        public DateTime? EmploymentDate { get; set; }
    }
}
