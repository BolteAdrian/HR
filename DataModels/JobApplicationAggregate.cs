using System;
using System.ComponentModel.DataAnnotations;
using HR.Models;

namespace HR.DataModels
{
    public class JobApplicationAggregate
    {
        //person
        public DateTime? DateApply { get; set; }
        public int? ModeApply { get; set; }
        public string CountyAddress { get; set; }
        public string CityAddress { get; set; }
        public DateTime? BirthDate { get; set; }

        //interview
        public string Name { get; set; }
        public int Id { get; set; }
        public long CandidateId { get; set; }
        public DateTime? InterviewDate { get; set; }

        public int FunctionApply { get; set; }
        public int DepartamentApply { get; set; }

        public bool? Accepted { get; set; }
        [Required]
        public string TestResult { get; set; }
        public int? RefusedReason { get; set; }
        public string RefusedObservation { get; set; }
        public string Comments { get; set; }
        [Required]
        public DateTime? DateAnswer { get; set; }
        public int? OfferStatus { get; set; }

        public DateTime? EmploymentDate { get; set; }
        public string AddedBy { get; set; }
        public DateTime? AddedAt { get; set; }
        public string UpdatedBy { get; set; }
        public string UpdatedAt { get; set; }

        //interview team
        public int InterviewTeamId { get; set; }
        public int InterviewId { get; set; }
        public int? interviewEmployeeId { get; set; }

        //employee
        public int EmployeeId { get; set; }

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

        public virtual Candidates Candidate { get; set; }
        //Documents

        public string DocumentName { get; set; }
        public DateTime? DateAdded { get; set; }
        public string Observation { get; set; }


        //function
        public string FunctionName { get; set; }

        //department
        public string DepartmentName { get; set; }

    }
}
