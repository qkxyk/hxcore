using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HXCloud.Repository.Migrations
{
    public partial class addTypeClass : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClassId",
                table: "TypeModuleControl",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TypeClass",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Rank = table.Column<int>(type: "int", nullable: false),
                    TypeId = table.Column<int>(type: "int", nullable: false),
                    Create = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    Modify = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeClass", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TypeClass_Type_TypeId",
                        column: x => x.TypeId,
                        principalTable: "Type",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TypeModuleControl_ClassId",
                table: "TypeModuleControl",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_TypeClass_TypeId",
                table: "TypeClass",
                column: "TypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_TypeModuleControl_TypeClass_ClassId",
                table: "TypeModuleControl",
                column: "ClassId",
                principalTable: "TypeClass",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TypeModuleControl_TypeClass_ClassId",
                table: "TypeModuleControl");

            migrationBuilder.DropTable(
                name: "TypeClass");

            migrationBuilder.DropIndex(
                name: "IX_TypeModuleControl_ClassId",
                table: "TypeModuleControl");

            migrationBuilder.DropColumn(
                name: "ClassId",
                table: "TypeModuleControl");
        }
    }
}
