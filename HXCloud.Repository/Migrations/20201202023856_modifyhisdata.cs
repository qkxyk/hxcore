using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HXCloud.Repository.Migrations
{
    public partial class modifyhisdata : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Create",
                table: "DeviceHisData");

            migrationBuilder.DropColumn(
                name: "CreateTime",
                table: "DeviceHisData");

            migrationBuilder.DropColumn(
                name: "Modify",
                table: "DeviceHisData");

            migrationBuilder.DropColumn(
                name: "ModifyTime",
                table: "DeviceHisData");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Create",
                table: "DeviceHisData",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateTime",
                table: "DeviceHisData",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "getdate()");

            migrationBuilder.AddColumn<string>(
                name: "Modify",
                table: "DeviceHisData",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifyTime",
                table: "DeviceHisData",
                type: "datetime2",
                nullable: true);
        }
    }
}
