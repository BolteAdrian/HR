using System.ComponentModel.DataAnnotations;

namespace HR.Models
{
    public partial class Departments
    {
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
