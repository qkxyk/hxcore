using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HXCloud.Repository.Migrations
{
    public partial class adddiscretestatistics : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TypeAccessoryControlData");

            migrationBuilder.DropTable(
                name: "TypeSystemAccessoryControlData");

            migrationBuilder.DropTable(
                name: "TypeAccessory");

            migrationBuilder.DropTable(
                name: "TypeSystemAccessory");

            migrationBuilder.DropTable(
                name: "TypeSystem");

            migrationBuilder.CreateTable(
                name: "DeviceDiscreteStatisticsData",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DeviceSn = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Data = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProjectId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceDiscreteStatisticsData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeviceDiscreteStatisticsData_Device_DeviceSn",
                        column: x => x.DeviceSn,
                        principalTable: "Device",
                        principalColumn: "DeviceSn",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DeviceDiscreteStatisticsData_DeviceSn",
                table: "DeviceDiscreteStatisticsData",
                column: "DeviceSn");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeviceDiscreteStatisticsData");

            migrationBuilder.CreateTable(
                name: "TypeAccessory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Create = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    ICON = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Modify = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeAccessory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TypeAccessory_Type_TypeId",
                        column: x => x.TypeId,
                        principalTable: "Type",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TypeSystem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Create = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    Modify = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Order = table.Column<int>(type: "int", nullable: false),
                    TypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeSystem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TypeSystem_Type_TypeId",
                        column: x => x.TypeId,
                        principalTable: "Type",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TypeAccessoryControlData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccessoryId = table.Column<int>(type: "int", nullable: false),
                    AssociateDefineId = table.Column<int>(type: "int", nullable: false),
                    ControlName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Create = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    DataDefineId = table.Column<int>(type: "int", nullable: false),
                    DataValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IState = table.Column<int>(type: "int", nullable: false),
                    Modify = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SequenceIn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SequenceOut = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeAccessoryControlData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TypeAccessoryControlData_TypeAccessory_AccessoryId",
                        column: x => x.AccessoryId,
                        principalTable: "TypeAccessory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TypeAccessoryControlData_TypeDataDefine_DataDefineId",
                        column: x => x.DataDefineId,
                        principalTable: "TypeDataDefine",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TypeSystemAccessory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Create = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    ICON = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Modify = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SystemId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeSystemAccessory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TypeSystemAccessory_TypeSystem_SystemId",
                        column: x => x.SystemId,
                        principalTable: "TypeSystem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TypeSystemAccessoryControlData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccessoryId = table.Column<int>(type: "int", nullable: false),
                    AssociateDefineId = table.Column<int>(type: "int", nullable: false),
                    ControlName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Create = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    DataDefineId = table.Column<int>(type: "int", nullable: false),
                    DataValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IState = table.Column<int>(type: "int", nullable: false),
                    Modify = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifyTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SequenceIn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SequenceOut = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeSystemAccessoryControlData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TypeSystemAccessoryControlData_TypeDataDefine_DataDefineId",
                        column: x => x.DataDefineId,
                        principalTable: "TypeDataDefine",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TypeSystemAccessoryControlData_TypeSystemAccessory_AccessoryId",
                        column: x => x.AccessoryId,
                        principalTable: "TypeSystemAccessory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TypeAccessory_TypeId",
                table: "TypeAccessory",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TypeAccessoryControlData_AccessoryId",
                table: "TypeAccessoryControlData",
                column: "AccessoryId");

            migrationBuilder.CreateIndex(
                name: "IX_TypeAccessoryControlData_DataDefineId",
                table: "TypeAccessoryControlData",
                column: "DataDefineId");

            migrationBuilder.CreateIndex(
                name: "IX_TypeSystem_TypeId",
                table: "TypeSystem",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TypeSystemAccessory_SystemId",
                table: "TypeSystemAccessory",
                column: "SystemId");

            migrationBuilder.CreateIndex(
                name: "IX_TypeSystemAccessoryControlData_AccessoryId",
                table: "TypeSystemAccessoryControlData",
                column: "AccessoryId");

            migrationBuilder.CreateIndex(
                name: "IX_TypeSystemAccessoryControlData_DataDefineId",
                table: "TypeSystemAccessoryControlData",
                column: "DataDefineId");
        }
    }
}
