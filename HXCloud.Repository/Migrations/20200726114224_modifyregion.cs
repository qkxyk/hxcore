using Microsoft.EntityFrameworkCore.Migrations;

namespace HXCloud.Repository.Migrations
{
    public partial class modifyregion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Region_Region_ParentId",
                table: "Region");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Region",
                table: "Region");

            migrationBuilder.DropIndex(
                name: "IX_Region_ParentId",
                table: "Region");

            migrationBuilder.AlterColumn<string>(
                name: "GroupId",
                table: "Region",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeleteId",
                table: "Region",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Region",
                table: "Region",
                columns: new[] { "Id", "GroupId" });

            migrationBuilder.CreateIndex(
                name: "IX_Region_ParentId_GroupId",
                table: "Region",
                columns: new[] { "ParentId", "GroupId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Region_Region_ParentId_GroupId",
                table: "Region",
                columns: new[] { "ParentId", "GroupId" },
                principalTable: "Region",
                principalColumns: new[] { "Id", "GroupId" },
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Region_Region_ParentId_GroupId",
                table: "Region");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Region",
                table: "Region");

            migrationBuilder.DropIndex(
                name: "IX_Region_ParentId_GroupId",
                table: "Region");

            migrationBuilder.DropColumn(
                name: "DeleteId",
                table: "Region");

            migrationBuilder.AlterColumn<string>(
                name: "GroupId",
                table: "Region",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Region",
                table: "Region",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Region_ParentId",
                table: "Region",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Region_Region_ParentId",
                table: "Region",
                column: "ParentId",
                principalTable: "Region",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
