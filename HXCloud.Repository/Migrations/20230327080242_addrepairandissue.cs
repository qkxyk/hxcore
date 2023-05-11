using Microsoft.EntityFrameworkCore.Migrations;

namespace HXCloud.Repository.Migrations
{
    public partial class addrepairandissue : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Repairs_IssueId",
                table: "Repairs",
                column: "IssueId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Repairs_Issue_IssueId",
                table: "Repairs",
                column: "IssueId",
                principalTable: "Issue",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Repairs_Issue_IssueId",
                table: "Repairs");

            migrationBuilder.DropIndex(
                name: "IX_Repairs_IssueId",
                table: "Repairs");
        }
    }
}
