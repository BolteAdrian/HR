using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HR.Models
{
    public partial class Candidates
    {
        public Candidates()
        {
            Interview = new HashSet<Interviews>();
        }

        public long Id { get; set; }
        [Required]
        public string Name { get; set; }
        public DateTime? DateApply { get; set; }
        [Required]
        public long? FunctionApply { get; set; }
        public long? FunctionMatch { get; set; }
        public string Observation { get; set; }
       
        public string Experience { get; set; }
      
        public string Studies { get; set; }
     
        public int? ModeApply { get; set; }
        [Required]
        public string County { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public DateTime? BirthDate { get; set; }
        public int? Age { get; set; }
        public string AddedBy { get; set; }
        public DateTime? AddedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? Status { get; set; }
        public DateTime? ReciveDate { get; set; }

        public virtual ICollection<Interviews> Interview { get; set; }
    }
}
