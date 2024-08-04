using System;

namespace HR.Models
{
    public partial class Predictions
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public DateTime? BirthDate { get; set; }
        public string FunctionName { get; set; }
        public string DepartmentName { get; set; }
        public string Studies { get; set; }
        public string Experience { get; set; }
        public string Observation { get; set; }
        public int? ModeApply { get; set; }
        public int? OfferStatus { get; set; }
    }
}
