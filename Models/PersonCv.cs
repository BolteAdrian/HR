using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace HR.Models
{
    public partial class PersonCv
    {
        public PersonCv()
        {
            Documents = new HashSet<Documents>();
            InterviewCv = new HashSet<InterviewCv>();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public DateTime? DateApply { get; set; }
        public string FunctionApply { get; set; }
        public string FunctionMatch { get; set; }
        public string Observation { get; set; }
        public int? ModeApply { get; set; }
        public string CountyAddress { get; set; }
        public string CityAddress { get; set; }
        public DateTime? BirthDate { get; set; }
        public int? Age { get; set; }
        public string AddedBy { get; set; }
        public DateTime? AddedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? Status { get; set; }
        public DateTime? CvreciveDate { get; set; }

        public virtual ICollection<Documents> Documents { get; set; }
        public virtual ICollection<InterviewCv> InterviewCv { get; set; }
    }
}
