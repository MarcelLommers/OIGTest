using Microsoft.EntityFrameworkCore.Migrations;

namespace OIG_Test.Migrations
{
    public partial class FixCapitalisation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "startDate",
                table: "Research",
                newName: "StartDate");

            migrationBuilder.RenameColumn(
                name: "endDate",
                table: "Research",
                newName: "EndDate");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Research",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "Research",
                newName: "startDate");

            migrationBuilder.RenameColumn(
                name: "EndDate",
                table: "Research",
                newName: "endDate");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Research",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
