using Microsoft.EntityFrameworkCore.Migrations;

namespace HXCloud.Repository.Migrations
{
    public partial class removeRepairIssue : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Repairs_Issue_IssueId",
                table: "Repairs");

            migrationBuilder.DropIndex(
                name: "IX_Repairs_IssueId",
                table: "Repairs");

            migrationBuilder.AddColumn<string>(
                name: "RepairId",
                table: "Issue",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Issue_RepairId",
                table: "Issue",
                column: "RepairId");

            migrationBuilder.AddForeignKey(
                name: "FK_Issue_Repairs_RepairId",
                table: "Issue",
                column: "RepairId",
                principalTable: "Repairs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Issue_Repairs_RepairId",
                table: "Issue");

            migrationBuilder.DropIndex(
                name: "IX_Issue_RepairId",
                table: "Issue");

            migrationBuilder.DropColumn(
                name: "RepairId",
                table: "Issue");

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
    }
}
