using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HXCloud.Repository.Migrations
{
    public partial class addwarn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WarnType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Create = table.Column<string>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()"),
                    Modify = table.Column<string>(nullable: true),
                    ModifyTime = table.Column<DateTime>(nullable: true),
                    TypeName = table.Column<string>(nullable: true),
                    Color = table.Column<string>(nullable: true),
                    Icon = table.Column<string>(nullable: true),
                    GroupId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WarnType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WarnType_Group_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Group",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WarnCode",
                columns: table => new
                {
                    Code = table.Column<string>(nullable: false),
                    WarnTypeId = table.Column<int>(nullable: false),
                    Create = table.Column<string>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()"),
                    Modify = table.Column<string>(nullable: true),
                    ModifyTime = table.Column<DateTime>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WarnCode", x => new { x.Code, x.WarnTypeId });
                    table.ForeignKey(
                        name: "FK_WarnCode_WarnType_WarnTypeId",
                        column: x => x.WarnTypeId,
                        principalTable: "WarnType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Warn",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Modify = table.Column<string>(nullable: true),
                    ModifyTime = table.Column<DateTime>(nullable: true),
                    Code = table.Column<string>(nullable: true),
                    WarnTypeId = table.Column<int>(nullable: false),
                    Dt = table.Column<DateTime>(nullable: false),
                    DeviceSn = table.Column<string>(nullable: true),
                    DeviceNo = table.Column<string>(nullable: true),
                    State = table.Column<bool>(nullable: false),
                    Comments = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Warn", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Warn_Device_DeviceSn",
                        column: x => x.DeviceSn,
                        principalTable: "Device",
                        principalColumn: "DeviceSn",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Warn_WarnCode_Code_WarnTypeId",
                        columns: x => new { x.Code, x.WarnTypeId },
                        principalTable: "WarnCode",
                        principalColumns: new[] { "Code", "WarnTypeId" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Warn_DeviceSn",
                table: "Warn",
                column: "DeviceSn");

            migrationBuilder.CreateIndex(
                name: "IX_Warn_Code_WarnTypeId",
                table: "Warn",
                columns: new[] { "Code", "WarnTypeId" });

            migrationBuilder.CreateIndex(
                name: "IX_WarnCode_WarnTypeId",
                table: "WarnCode",
                column: "WarnTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_WarnType_GroupId",
                table: "WarnType",
                column: "GroupId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Warn");

            migrationBuilder.DropTable(
                name: "WarnCode");

            migrationBuilder.DropTable(
                name: "WarnType");
        }
    }
}
