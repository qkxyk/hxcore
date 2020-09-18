using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HXCloud.Repository.Migrations
{
    public partial class AddTypeDisplayIcon : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TypeDisplayIcon",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Create = table.Column<string>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Icon = table.Column<string>(nullable: true),
                    TypeId = table.Column<int>(nullable: false),
                    DataDefineId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeDisplayIcon", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TypeDisplayIcon_TypeDataDefine_DataDefineId",
                        column: x => x.DataDefineId,
                        principalTable: "TypeDataDefine",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TypeDisplayIcon_Type_TypeId",
                        column: x => x.TypeId,
                        principalTable: "Type",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TypeDisplayIcon_DataDefineId",
                table: "TypeDisplayIcon",
                column: "DataDefineId");

            migrationBuilder.CreateIndex(
                name: "IX_TypeDisplayIcon_TypeId",
                table: "TypeDisplayIcon",
                column: "TypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TypeDisplayIcon");
        }
    }
}
