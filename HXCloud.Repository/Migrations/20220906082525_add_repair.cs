using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HXCloud.Repository.Migrations
{
    public partial class add_repair : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CheckDescription",
                table: "Repairs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CheckTime",
                table: "Repairs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Repairs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReceivePhone",
                table: "Repairs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ReceiveTime",
                table: "Repairs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Receiver",
                table: "Repairs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WaitDescription",
                table: "Repairs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "WaitTime",
                table: "Repairs",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CheckDescription",
                table: "Repairs");

            migrationBuilder.DropColumn(
                name: "CheckTime",
                table: "Repairs");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Repairs");

            migrationBuilder.DropColumn(
                name: "ReceivePhone",
                table: "Repairs");

            migrationBuilder.DropColumn(
                name: "ReceiveTime",
                table: "Repairs");

            migrationBuilder.DropColumn(
                name: "Receiver",
                table: "Repairs");

            migrationBuilder.DropColumn(
                name: "WaitDescription",
                table: "Repairs");

            migrationBuilder.DropColumn(
                name: "WaitTime",
                table: "Repairs");
        }
    }
}
