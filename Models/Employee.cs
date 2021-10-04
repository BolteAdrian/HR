using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace HR.Models
{
    public partial class Employee
    {
        public Employee()
        {
            InterviewTeam = new HashSet<InterviewTeam>();
        }

        public int Id { get; set; }
        public int? EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public int OrganizationId { get; set; }
        public int? CorId { get; set; }
        public DateTime? EmploymentDate { get; set; }
        public DateTime? EndDate { get; set; }
        public short? IsActive { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public int Plant { get; set; }
        public string Team { get; set; }
        public string CompanyShortName { get; set; }

        public virtual ICollection<InterviewTeam> InterviewTeam { get; set; }
    }
}
