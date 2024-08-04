using Microsoft.EntityFrameworkCore.Migrations;

namespace HR.Migrations
{
    public partial class AddRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO AspNetRoles (Id, [Name], NormalizedName) VALUES ('1', 'Admin', 'ADMIN')");
            migrationBuilder.Sql("INSERT INTO AspNetRoles (Id, [Name], NormalizedName) VALUES ('2', 'User', 'USER')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
