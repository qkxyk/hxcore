using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HXCloud.Repository.Migrations
{
    public partial class addDeviceDayMonitorData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DeviceDayMonitorData",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WaterType = table.Column<int>(type: "int", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TimeSn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeviceSn = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Mn = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceDayMonitorData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeviceDayMonitorData_Device_DeviceSn",
                        column: x => x.DeviceSn,
                        principalTable: "Device",
                        principalColumn: "DeviceSn",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DeviceDayMonitorData_DeviceSn",
                table: "DeviceDayMonitorData",
                column: "DeviceSn");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeviceDayMonitorData");
        }
    }
}
