using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HXCloud.Repository.Migrations
{
    public partial class updateTypeCraftTop : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TypeCraftTopModel_Type_TypeId",
                table: "TypeCraftTopModel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TypeCraftTopModel",
                table: "TypeCraftTopModel");

            migrationBuilder.RenameTable(
                name: "TypeCraftTopModel",
                newName: "TypeCraftTop");

            migrationBuilder.RenameIndex(
                name: "IX_TypeCraftTopModel_TypeId",
                table: "TypeCraftTop",
                newName: "IX_TypeCraftTop_TypeId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateTime",
                table: "TypeCraftTop",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "getdate()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TypeCraftTop",
                table: "TypeCraftTop",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TypeCraftTop_Type_TypeId",
                table: "TypeCraftTop",
                column: "TypeId",
                principalTable: "Type",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TypeCraftTop_Type_TypeId",
                table: "TypeCraftTop");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TypeCraftTop",
                table: "TypeCraftTop");

            migrationBuilder.RenameTable(
                name: "TypeCraftTop",
                newName: "TypeCraftTopModel");

            migrationBuilder.RenameIndex(
                name: "IX_TypeCraftTop_TypeId",
                table: "TypeCraftTopModel",
                newName: "IX_TypeCraftTopModel_TypeId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateTime",
                table: "TypeCraftTopModel",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "getdate()");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TypeCraftTopModel",
                table: "TypeCraftTopModel",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TypeCraftTopModel_Type_TypeId",
                table: "TypeCraftTopModel",
                column: "TypeId",
                principalTable: "Type",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
