using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HR.Models
{
    public partial class Employees
    {
        public Employees()
        {
            InterviewTeam = new HashSet<InterviewTeams>();
        }

        public int Id { get; set; }
        [Required]
        public int? EmployeeId { get; set; }
        [Required]
        public string Name { get; set; }
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

        public virtual ICollection<InterviewTeams> InterviewTeam { get; set; }
    }
}
