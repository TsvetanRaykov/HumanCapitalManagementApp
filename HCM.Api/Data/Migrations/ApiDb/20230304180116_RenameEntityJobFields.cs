using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HCM.Api.Data.Migrations.ApiDb
{
    public partial class RenameEntityJobFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "JobTitle",
                table: "Jobs",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "JobDescription",
                table: "Jobs",
                newName: "Description");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Jobs",
                newName: "JobTitle");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Jobs",
                newName: "JobDescription");
        }
    }
}
