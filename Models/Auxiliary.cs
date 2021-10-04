using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace HR.Models
{
    public partial class Auxiliary
    {
        public short Id { get; set; }
        public string ParameterName { get; set; }
        public string ParameterKey { get; set; }
        public string ParameterValue { get; set; }
        public string Language { get; set; }
    }
}
