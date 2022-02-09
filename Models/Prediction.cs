using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace HR.Models
{
    public partial class Prediction
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public DateTime? BirthDate { get; set; }
        public string NameFunction { get; set; }
        public string NameDepartment { get; set; }
        public string Studies { get; set; }
        public string Experience { get; set; }
        public string Observation { get; set; }
        public int? ModeApply { get; set; }
        public int? OffertStatus { get; set; }
    }
}
