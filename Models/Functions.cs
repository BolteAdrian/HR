using System.ComponentModel.DataAnnotations;

namespace HR.Models
{
    public partial class Functions
    {
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }
        public long DepartmentId { get; set; }
    }
}
