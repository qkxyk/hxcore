using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HXCloud.Repository.Migrations
{
    public partial class addTypeModule : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TypeModule",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Create = table.Column<string>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()"),
                    Modify = table.Column<string>(nullable: true),
                    ModifyTime = table.Column<DateTime>(nullable: true),
                    ModuleName = table.Column<string>(nullable: true),
                    ModuleType = table.Column<int>(nullable: false),
                    Sn = table.Column<int>(nullable: false),
                    TypeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeModule", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TypeModule_Type_TypeId",
                        column: x => x.TypeId,
                        principalTable: "Type",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TypeModuleControl",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Create = table.Column<string>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()"),
                    Modify = table.Column<string>(nullable: true),
                    ModifyTime = table.Column<DateTime>(nullable: true),
                    ControlName = table.Column<string>(nullable: true),
                    DataValue = table.Column<string>(nullable: true),
                    Formula = table.Column<string>(nullable: true),
                    Sn = table.Column<int>(nullable: false),
                    DataDefineId = table.Column<int>(nullable: false),
                    ModuleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeModuleControl", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TypeModuleControl_TypeDataDefine_DataDefineId",
                        column: x => x.DataDefineId,
                        principalTable: "TypeDataDefine",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TypeModuleControl_TypeModule_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "TypeModule",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TypeModuleFeedback",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Create = table.Column<string>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()"),
                    Modify = table.Column<string>(nullable: true),
                    ModifyTime = table.Column<DateTime>(nullable: true),
                    Sn = table.Column<int>(nullable: false),
                    ModuleControlId = table.Column<int>(nullable: false),
                    DataDefineId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeModuleFeedback", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TypeModuleFeedback_TypeDataDefine_DataDefineId",
                        column: x => x.DataDefineId,
                        principalTable: "TypeDataDefine",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TypeModuleFeedback_TypeModuleControl_ModuleControlId",
                        column: x => x.ModuleControlId,
                        principalTable: "TypeModuleControl",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TypeModule_TypeId",
                table: "TypeModule",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TypeModuleControl_DataDefineId",
                table: "TypeModuleControl",
                column: "DataDefineId");

            migrationBuilder.CreateIndex(
                name: "IX_TypeModuleControl_ModuleId",
                table: "TypeModuleControl",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_TypeModuleFeedback_DataDefineId",
                table: "TypeModuleFeedback",
                column: "DataDefineId");

            migrationBuilder.CreateIndex(
                name: "IX_TypeModuleFeedback_ModuleControlId",
                table: "TypeModuleFeedback",
                column: "ModuleControlId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TypeModuleFeedback");

            migrationBuilder.DropTable(
                name: "TypeModuleControl");

            migrationBuilder.DropTable(
                name: "TypeModule");
        }
    }
}
