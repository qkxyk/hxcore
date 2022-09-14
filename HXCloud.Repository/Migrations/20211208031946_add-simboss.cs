using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HXCloud.Repository.Migrations
{
    public partial class addsimboss : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Simboss",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SimAccount = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SimPassword = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AppId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AppSecret = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Create = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    Modify = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Simboss", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Simboss");
        }
    }
}
