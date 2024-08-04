using System;
using System.ComponentModel.DataAnnotations;

namespace HR.Models
{
    public partial class Documents
    {
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }
        public DateTime DateAdded { get; set; }
        public string Observation { get; set; }
        public long CandidateId { get; set; }
    }
}
