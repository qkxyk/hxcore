using Microsoft.EntityFrameworkCore.Migrations;

namespace HXCloud.Repository.Migrations
{
    public partial class modifyCraftComponentCatalog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Icon",
                table: "CraftElement",
                newName: "Image");

            migrationBuilder.AddColumn<int>(
                name: "CraftCatalogType",
                table: "CraftComponentCatelog",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ElementType",
                table: "CraftComponentCatelog",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ParentId",
                table: "CraftComponentCatelog",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CraftComponentCatelog_ParentId",
                table: "CraftComponentCatelog",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_CraftComponentCatelog_CraftComponentCatelog_ParentId",
                table: "CraftComponentCatelog",
                column: "ParentId",
                principalTable: "CraftComponentCatelog",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CraftComponentCatelog_CraftComponentCatelog_ParentId",
                table: "CraftComponentCatelog");

            migrationBuilder.DropIndex(
                name: "IX_CraftComponentCatelog_ParentId",
                table: "CraftComponentCatelog");

            migrationBuilder.DropColumn(
                name: "CraftCatalogType",
                table: "CraftComponentCatelog");

            migrationBuilder.DropColumn(
                name: "ElementType",
                table: "CraftComponentCatelog");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "CraftComponentCatelog");

            migrationBuilder.RenameColumn(
                name: "Image",
                table: "CraftElement",
                newName: "Icon");
        }
    }
}
