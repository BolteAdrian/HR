using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace HR.Models
{
    public partial class Auxi
    {
        public long Id { get; set; }
        public string RefusedReason { get; set; }
        public string OffertStatus { get; set; }
        public string ModeApply { get; set; }
        public string Status { get; set; }
        public string FunctionCv { get; set; }
        public string Department { get; set; }
        public string Accepted { get; set; }
    }
}
