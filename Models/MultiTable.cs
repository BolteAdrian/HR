using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HR.Models
{
    public class MultiTable
    {


        //person
        public DateTime? DateApply { get; set; }
        public int? ModeApply { get; set; }
        public string CountyAddress { get; set; }
        public string CityAddress { get; set; }
        public DateTime? BirthDate { get; set; }

        //interview
        public string Name { get; set; }
        public long Id { get; set; }
        public long PersonCvid { get; set; }
        public DateTime? InterviewDate { get; set; }
       
        public string FunctionApply { get; set; }
        public string DepartamentApply { get; set; }
        
        public int? Accepted { get; set; }
        [Required]
        public string? TestResult { get; set; }
        public int? RefusedReason { get; set; }
        public string RefusedObservation { get; set; }
        public string Comments { get; set; }
        [Required]
        public DateTime? DateAnswer { get; set; }
        public int? OffertStatus { get; set; }
        [Required]
        public DateTime? EmploymentDate { get; set; }
        public string AddedBy { get; set; }
        public DateTime? AddedAt { get; set; }
        public string UpdatedBy { get; set; }
        public string UpdatedAt { get; set; }

        //interview team
        public int Id2 { get; set; }
        public long InterviewCvid { get; set; }
        public int? EmployeeId { get; set; }

        //employee
        public int IdEmployee { get; set; }

        public string EmployeeName { get; set; }
        public int OrganizationId { get; set; }
        public int? CorId { get; set; }

        public DateTime? EndDate { get; set; }
        public short? IsActive { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public int Plant { get; set; }
        public string Team { get; set; }
        public string CompanyShortName { get; set; }

        public virtual PersonCv PersonCv { get; set; }
        //Dcouments

        public string DocumentName { get; set; }
        public DateTime? DateAdded { get; set; }
        public string Observation { get; set; }




    }
}
