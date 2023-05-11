using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HXCloud.Repository.Migrations
{
    public partial class addrepairdata : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CheckAccount",
                table: "Repairs");

            migrationBuilder.DropColumn(
                name: "CheckAccountName",
                table: "Repairs");

            migrationBuilder.DropColumn(
                name: "CheckDescription",
                table: "Repairs");

            migrationBuilder.DropColumn(
                name: "CheckTime",
                table: "Repairs");

            migrationBuilder.DropColumn(
                name: "ReceiveTime",
                table: "Repairs");

            migrationBuilder.DropColumn(
                name: "Url",
                table: "Repairs");

            migrationBuilder.DropColumn(
                name: "WaitDescription",
                table: "Repairs");

            migrationBuilder.DropColumn(
                name: "WaitTime",
                table: "Repairs");

            migrationBuilder.AlterColumn<string>(
                name: "RepairType",
                table: "Repairs",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "RepairStatus",
                table: "Repairs",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "EmergenceStatus",
                table: "Repairs",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<bool>(
                name: "IsComplete",
                table: "Repairs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsParts",
                table: "Repairs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "RepairData",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RepairId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Sn = table.Column<int>(type: "int", nullable: false),
                    Operator = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OperDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RepairStatus = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RepairData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RepairData_Repairs_RepairId",
                        column: x => x.RepairId,
                        principalTable: "Repairs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RepairPart",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RepairId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Num = table.Column<int>(type: "int", nullable: false),
                    Data = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Operate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OperateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RepairPart", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RepairPart_Repairs_RepairId",
                        column: x => x.RepairId,
                        principalTable: "Repairs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RepairData_RepairId",
                table: "RepairData",
                column: "RepairId");

            migrationBuilder.CreateIndex(
                name: "IX_RepairPart_RepairId",
                table: "RepairPart",
                column: "RepairId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RepairData");

            migrationBuilder.DropTable(
                name: "RepairPart");

            migrationBuilder.DropColumn(
                name: "IsComplete",
                table: "Repairs");

            migrationBuilder.DropColumn(
                name: "IsParts",
                table: "Repairs");

            migrationBuilder.AlterColumn<int>(
                name: "RepairType",
                table: "Repairs",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "RepairStatus",
                table: "Repairs",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "EmergenceStatus",
                table: "Repairs",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "CheckAccount",
                table: "Repairs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CheckAccountName",
                table: "Repairs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CheckDescription",
                table: "Repairs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CheckTime",
                table: "Repairs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ReceiveTime",
                table: "Repairs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "Repairs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WaitDescription",
                table: "Repairs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "WaitTime",
                table: "Repairs",
                type: "datetime2",
                nullable: true);
        }
    }
}
