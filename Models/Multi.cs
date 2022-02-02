using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace HR.Models
{
    public partial class Multi
    {
        public long Id { get; set; }
        public long IdP { get; set; }
        public string Name { get; set; }
        public DateTime? InterviewDate { get; set; }
        public string NameFunction { get; set; }
        public string NameDepartment { get; set; }
        public int? EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public bool? Accepted { get; set; }
        public string TestResult { get; set; }
        public int? RefusedReason { get; set; }
        public string Comments { get; set; }
        public string Observation { get; set; }
        public DateTime? DateAnswer { get; set; }
        public int? OffertStatus { get; set; }
        public DateTime? EmploymentDate { get; set; }

    }
}
