using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HXCloud.Repository.Migrations
{
    public partial class modifyDeviceStatistics : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Create",
                table: "DeviceStatisticsData");

            migrationBuilder.DropColumn(
                name: "CreateTime",
                table: "DeviceStatisticsData");

            migrationBuilder.DropColumn(
                name: "Modify",
                table: "DeviceStatisticsData");

            migrationBuilder.DropColumn(
                name: "ModifyTime",
                table: "DeviceStatisticsData");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Create",
                table: "DeviceStatisticsData",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateTime",
                table: "DeviceStatisticsData",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "getdate()");

            migrationBuilder.AddColumn<string>(
                name: "Modify",
                table: "DeviceStatisticsData",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifyTime",
                table: "DeviceStatisticsData",
                type: "datetime2",
                nullable: true);
        }
    }
}
