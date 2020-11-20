using Microsoft.EntityFrameworkCore.Migrations;

namespace HXCloud.Repository.Migrations
{
    public partial class updateTypeArgument : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TypeArgument_TypeDataDefine_TypeDataDefineId",
                table: "TypeArgument");

            migrationBuilder.DropIndex(
                name: "IX_TypeArgument_TypeDataDefineId",
                table: "TypeArgument");

            migrationBuilder.DropColumn(
                name: "TypeDataDefineId",
                table: "TypeArgument");

            migrationBuilder.CreateIndex(
                name: "IX_TypeArgument_DefineId",
                table: "TypeArgument",
                column: "DefineId");

            migrationBuilder.AddForeignKey(
                name: "FK_TypeArgument_TypeDataDefine_DefineId",
                table: "TypeArgument",
                column: "DefineId",
                principalTable: "TypeDataDefine",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TypeArgument_TypeDataDefine_DefineId",
                table: "TypeArgument");

            migrationBuilder.DropIndex(
                name: "IX_TypeArgument_DefineId",
                table: "TypeArgument");

            migrationBuilder.AddColumn<int>(
                name: "TypeDataDefineId",
                table: "TypeArgument",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TypeArgument_TypeDataDefineId",
                table: "TypeArgument",
                column: "TypeDataDefineId");

            migrationBuilder.AddForeignKey(
                name: "FK_TypeArgument_TypeDataDefine_TypeDataDefineId",
                table: "TypeArgument",
                column: "TypeDataDefineId",
                principalTable: "TypeDataDefine",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
