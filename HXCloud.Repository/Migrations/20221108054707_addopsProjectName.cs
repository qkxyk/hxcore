using Microsoft.EntityFrameworkCore.Migrations;

namespace HXCloud.Repository.Migrations
{
    public partial class addopsProjectName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProjectName",
                table: "Repairs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreateName",
                table: "PatrolData",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProjectName",
                table: "PatrolData",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreateName",
                table: "Issue",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HandleName",
                table: "Issue",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProjectName",
                table: "Issue",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProjectName",
                table: "Repairs");

            migrationBuilder.DropColumn(
                name: "CreateName",
                table: "PatrolData");

            migrationBuilder.DropColumn(
                name: "ProjectName",
                table: "PatrolData");

            migrationBuilder.DropColumn(
                name: "CreateName",
                table: "Issue");

            migrationBuilder.DropColumn(
                name: "HandleName",
                table: "Issue");

            migrationBuilder.DropColumn(
                name: "ProjectName",
                table: "Issue");
        }
    }
}
