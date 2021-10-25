using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace HR.Models
{
    public partial class Multi
    {
        public long Id { get; set; }
        public string Name { get; set; }
        [Required]
        public DateTime? InterviewDate { get; set; }
        [Required]
        public string FunctionApply { get; set; }
        public string DepartamentApply { get; set; }
        public string EmployeeName { get; set; }
        [Required]
        public bool? Accepted { get; set; }
        public string TestResult { get; set; }
        public int? RefusedReason { get; set; }
        public DateTime? DateAnswer { get; set; }
        [Required]
        public int? OffertStatus { get; set; }
        public DateTime? EmploymentDate { get; set; }
    }
}
