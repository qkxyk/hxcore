using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HXCloud.Repository.Migrations
{
    public partial class box : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HXBox",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UUId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Activate = table.Column<bool>(type: "bit", nullable: false),
                    Num = table.Column<int>(type: "int", nullable: false),
                    Create = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    Modify = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HXBox", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HXBox");
        }
    }
}
