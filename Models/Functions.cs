using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace HR.Models
{
    public partial class Functions
    {
        public long Id { get; set; }
        [Required]
        public string NameFunction { get; set; }
        public long IdDepartment { get; set; }
    }
}
