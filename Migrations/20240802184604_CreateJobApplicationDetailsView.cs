using Microsoft.EntityFrameworkCore.Migrations;

namespace HR.Migrations
{
    public partial class CreateJobApplicationDetailsView : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            CREATE VIEW JobApplicationDetails AS
            SELECT 
                i.[Id],
                i.[CandidateId],
                c.[Name] AS CandidateName,
                i.[InterviewDate],
                f.[Name] AS [Function],
                d.[Name] AS Department,
                e.[EmployeeId],
                e.[Name] AS EmployeeName,
                i.[Accepted],
                i.[TestResult],
                i.[RefusedReason],
                i.[Comments],
                i.[DateAnswer],
                i.[OfferStatus],
                i.[EmploymentDate]
            FROM 
                [HRDB].[dbo].[Interviews] i
            INNER JOIN 
                [HRDB].[dbo].[Candidates] c ON c.[Id] = i.[CandidateId]
            INNER JOIN 
                [HRDB].[dbo].[Functions] f ON f.[Id] = i.[FunctionApply]
            INNER JOIN 
                [HRDB].[dbo].[Departments] d ON f.[DepartmentId] = d.[Id]
            INNER JOIN 
                [HRDB].[dbo].[InterviewTeams] it ON it.[InterviewId] = i.[Id]
            INNER JOIN 
                [HRDB].[dbo].[Employees] e ON it.[EmployeeId] = e.[Id];
        ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
