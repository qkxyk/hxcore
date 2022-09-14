using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HXCloud.Repository.Migrations
{
    public partial class addops : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Category",
                table: "User",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ParentId",
                table: "User",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Issue",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DeviceSn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeviceName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Dt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    Opinion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Create = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    Modify = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Issue", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OpsItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Key = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OpsType = table.Column<int>(type: "int", nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Max = table.Column<float>(type: "real", nullable: false),
                    Min = table.Column<float>(type: "real", nullable: false),
                    Create = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Modify = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpsItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PatrolData",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Dt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeviceSn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeviceName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Position = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PositionName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Create = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    Modify = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatrolData", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Repairs",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DeviceSn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeviceName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IssueId = table.Column<int>(type: "int", nullable: false),
                    RepairType = table.Column<int>(type: "int", nullable: false),
                    RepairStatus = table.Column<int>(type: "int", nullable: false),
                    EmergenceStatus = table.Column<int>(type: "int", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompleteTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Create = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    Modify = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Repairs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TypeOpsItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Key = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OpsType = table.Column<int>(type: "int", nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Max = table.Column<float>(type: "real", nullable: false),
                    Min = table.Column<float>(type: "real", nullable: false),
                    TypeId = table.Column<int>(type: "int", nullable: false),
                    Create = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Modify = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeOpsItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TypeOpsItems_Type_TypeId",
                        column: x => x.TypeId,
                        principalTable: "Type",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DevicePatrol",
                columns: table => new
                {
                    PatrolId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DevicePatrol", x => x.PatrolId);
                    table.ForeignKey(
                        name: "FK_DevicePatrol_PatrolData_PatrolId",
                        column: x => x.PatrolId,
                        principalTable: "PatrolData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PatrolImage",
                columns: table => new
                {
                    PatrolId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatrolImage", x => x.PatrolId);
                    table.ForeignKey(
                        name: "FK_PatrolImage_PatrolData_PatrolId",
                        column: x => x.PatrolId,
                        principalTable: "PatrolData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductData",
                columns: table => new
                {
                    PatrolId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductData", x => x.PatrolId);
                    table.ForeignKey(
                        name: "FK_ProductData_PatrolData_PatrolId",
                        column: x => x.PatrolId,
                        principalTable: "PatrolData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TechniquePatrol",
                columns: table => new
                {
                    PatrolId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TechniquePatrol", x => x.PatrolId);
                    table.ForeignKey(
                        name: "FK_TechniquePatrol_PatrolData_PatrolId",
                        column: x => x.PatrolId,
                        principalTable: "PatrolData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WaterAnalysis",
                columns: table => new
                {
                    PatrolId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WaterAnalysis", x => x.PatrolId);
                    table.ForeignKey(
                        name: "FK_WaterAnalysis_PatrolData_PatrolId",
                        column: x => x.PatrolId,
                        principalTable: "PatrolData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TypeOpsItems_TypeId",
                table: "TypeOpsItems",
                column: "TypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DevicePatrol");

            migrationBuilder.DropTable(
                name: "Issue");

            migrationBuilder.DropTable(
                name: "OpsItems");

            migrationBuilder.DropTable(
                name: "PatrolImage");

            migrationBuilder.DropTable(
                name: "ProductData");

            migrationBuilder.DropTable(
                name: "Repairs");

            migrationBuilder.DropTable(
                name: "TechniquePatrol");

            migrationBuilder.DropTable(
                name: "TypeOpsItems");

            migrationBuilder.DropTable(
                name: "WaterAnalysis");

            migrationBuilder.DropTable(
                name: "PatrolData");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "User");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "User");
        }
    }
}
