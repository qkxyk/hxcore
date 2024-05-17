using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HXCloud.Repository.Migrations
{
    public partial class addopsfault : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FaultCode",
                table: "Repairs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "OpsFaultType",
                columns: table => new
                {
                    FaultTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Flag = table.Column<int>(type: "int", nullable: false),
                    FaultTypeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentId = table.Column<int>(type: "int", nullable: true),
                    Create = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    Modify = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpsFaultType", x => x.FaultTypeId);
                    table.ForeignKey(
                        name: "FK_OpsFaultType_OpsFaultType_ParentId",
                        column: x => x.ParentId,
                        principalTable: "OpsFaultType",
                        principalColumn: "FaultTypeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OpsFault",
                columns: table => new
                {
                    Code = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Solution = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OpsFaultTypeId = table.Column<int>(type: "int", nullable: false),
                    Create = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    Modify = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpsFault", x => x.Code);
                    table.ForeignKey(
                        name: "FK_OpsFault_OpsFaultType_OpsFaultTypeId",
                        column: x => x.OpsFaultTypeId,
                        principalTable: "OpsFaultType",
                        principalColumn: "FaultTypeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OpsFault_OpsFaultTypeId",
                table: "OpsFault",
                column: "OpsFaultTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_OpsFaultType_ParentId",
                table: "OpsFaultType",
                column: "ParentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OpsFault");

            migrationBuilder.DropTable(
                name: "OpsFaultType");

            migrationBuilder.DropColumn(
                name: "FaultCode",
                table: "Repairs");
        }
    }
}
