using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HCM.Api.Data.Migrations.ApiDb
{
    public partial class AddUniqueIndexEmailToEmployeeEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Employees_Email",
                table: "Employees",
                column: "Email",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Employees_Email",
                table: "Employees");
        }
    }
}
