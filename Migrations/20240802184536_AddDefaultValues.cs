using Microsoft.EntityFrameworkCore.Migrations;

namespace HR.Migrations
{
    public partial class AddDefaultValues : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO Departments ([Name]) VALUES ('No Department')");
            migrationBuilder.Sql("INSERT INTO Functions ([Name], DepartmentId) VALUES ('No Function', 1)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
