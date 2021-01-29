using Microsoft.EntityFrameworkCore.Migrations;

namespace HXCloud.Repository.Migrations
{
    public partial class devicecardaddposition : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_DeviceCard",
                table: "DeviceCard");

            migrationBuilder.AlterColumn<string>(
                name: "CardNo",
                table: "DeviceCard",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "DeviceCard",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<string>(
                name: "ICCID",
                table: "DeviceCard",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IMEI",
                table: "DeviceCard",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Latitude",
                table: "DeviceCard",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Longitude",
                table: "DeviceCard",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_DeviceCard",
                table: "DeviceCard",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_DeviceCard",
                table: "DeviceCard");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "DeviceCard");

            migrationBuilder.DropColumn(
                name: "ICCID",
                table: "DeviceCard");

            migrationBuilder.DropColumn(
                name: "IMEI",
                table: "DeviceCard");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "DeviceCard");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "DeviceCard");

            migrationBuilder.AlterColumn<string>(
                name: "CardNo",
                table: "DeviceCard",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_DeviceCard",
                table: "DeviceCard",
                column: "CardNo");
        }
    }
}
