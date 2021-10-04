using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HR.Models
{
    public class MultiTableGrid
    {
        //Person
        public PersonCv Id { get; set; }
        public long PersonCvid { get; set; }
        public PersonCv Name { get; set; }
        public PersonCv DateApply { get; set; }
        public PersonCv ModeApply { get; set; }
        public PersonCv CityAddress { get; set; }
        public PersonCv CountyAddress { get; set; }

        public PersonCv BirthDate { get; set; }
        public PersonCv Age { get; set; }
        public PersonCv Status { get; set; }
        public PersonCv CvreciveDate { get; set; }
        //Interview
        public InterviewCv IPersonCvid { get; set; }
        public InterviewCv InterviewDate { get; set; }
        public  InterviewCv FunctionApply { get; set; }
        public InterviewCv DepartamentApply { get; set; }
        public InterviewCv Accepted { get; set; }
        public InterviewCv TestResult { get; set; }
        public InterviewCv OffertStatus { get; set; }
        //
        public Employee EmployeeId { get; set; }
        public Employee EmployeeName { get; set; }
        public Employee EmploymentDate { get; set; }
        //Documents
        public   Documents DocumentName { get; set; }
        public Documents DateAdded { get; set; }
        public Documents PersonCv { get; set; }
        public Documents  Observation { get; set; }
        
    }
}
