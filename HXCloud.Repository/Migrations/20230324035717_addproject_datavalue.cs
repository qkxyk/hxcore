using Microsoft.EntityFrameworkCore.Migrations;

namespace HXCloud.Repository.Migrations
{
    public partial class addproject_datavalue : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Crafts",
                table: "Project",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeviceType",
                table: "Project",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ProjectScale_Num",
                table: "Project",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProjectScale_Unit",
                table: "Project",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WaterIndex",
                table: "Project",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Crafts",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "DeviceType",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "ProjectScale_Num",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "ProjectScale_Unit",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "WaterIndex",
                table: "Project");
        }
    }
}
