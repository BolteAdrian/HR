namespace HR.Models
{
    public partial class InterviewTeams
    {
        public long Id { get; set; }
        public long InterviewId{ get; set; }
        public int EmployeeId { get; set; }

        public virtual Employees Employee { get; set; }
        public virtual Interviews Interview { get; set; }
    }
}
