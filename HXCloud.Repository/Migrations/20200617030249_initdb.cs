using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HXCloud.Repository.Migrations
{
    public partial class initdb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppVersion",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Create = table.Column<string>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()"),
                    Modify = table.Column<string>(nullable: true),
                    ModifyTime = table.Column<DateTime>(nullable: true),
                    VersionNo = table.Column<string>(nullable: true),
                    Descrption = table.Column<string>(nullable: true),
                    State = table.Column<bool>(nullable: false),
                    Address = table.Column<string>(nullable: true),
                    Type = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppVersion", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DataDefineLibrary",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Create = table.Column<string>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()"),
                    Modify = table.Column<string>(nullable: true),
                    ModifyTime = table.Column<DateTime>(nullable: true),
                    DataKey = table.Column<string>(nullable: true),
                    DataName = table.Column<string>(nullable: true),
                    Unit = table.Column<string>(nullable: true),
                    DataType = table.Column<string>(nullable: true),
                    DefaultValue = table.Column<string>(nullable: true),
                    Format = table.Column<string>(nullable: true),
                    Model = table.Column<int>(nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataDefineLibrary", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Group",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Create = table.Column<string>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()"),
                    Modify = table.Column<string>(nullable: true),
                    ModifyTime = table.Column<DateTime>(nullable: true),
                    GroupName = table.Column<string>(nullable: true),
                    GroupCode = table.Column<string>(nullable: true),
                    Logo = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Group", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Department",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Create = table.Column<string>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()"),
                    Modify = table.Column<string>(nullable: true),
                    ModifyTime = table.Column<DateTime>(nullable: true),
                    DepartmentName = table.Column<string>(nullable: true),
                    ParentId = table.Column<int>(nullable: true),
                    Level = table.Column<int>(nullable: false, defaultValue: 0),
                    PathId = table.Column<string>(nullable: true),
                    PathName = table.Column<string>(nullable: true),
                    DepartmentType = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    GroupId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Department", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Department_Group_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Group",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Department_Department_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Department",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Project",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Create = table.Column<string>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()"),
                    Modify = table.Column<string>(nullable: true),
                    ModifyTime = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    ProjectType = table.Column<int>(nullable: false),
                    PathName = table.Column<string>(nullable: true),
                    PathId = table.Column<string>(nullable: true),
                    RegionId = table.Column<string>(nullable: true),
                    Position = table.Column<string>(nullable: true),
                    ParentId = table.Column<int>(nullable: true),
                    GroupId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Project", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Project_Group_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Group",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Project_Project_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Region",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Create = table.Column<string>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()"),
                    Modify = table.Column<string>(nullable: true),
                    ModifyTime = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Point = table.Column<string>(nullable: true),
                    Radius = table.Column<string>(nullable: true),
                    ParentId = table.Column<string>(nullable: true),
                    FullPath = table.Column<string>(nullable: true),
                    GroupId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Region", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Region_Group_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Group",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Region_Region_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Region",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Create = table.Column<string>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()"),
                    Modify = table.Column<string>(nullable: true),
                    ModifyTime = table.Column<DateTime>(nullable: true),
                    RoleName = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    IsAdmin = table.Column<bool>(nullable: false, defaultValue: false),
                    GroupId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Role_Group_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Group",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Type",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Create = table.Column<string>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()"),
                    Modify = table.Column<string>(nullable: true),
                    ModifyTime = table.Column<DateTime>(nullable: true),
                    TypeName = table.Column<string>(nullable: true),
                    ParentId = table.Column<int>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    PathId = table.Column<string>(nullable: true),
                    PathName = table.Column<string>(nullable: true),
                    ICON = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    GroupId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Type", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Type_Group_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Group",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Type_Type_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Type",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Create = table.Column<string>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()"),
                    Modify = table.Column<string>(nullable: true),
                    ModifyTime = table.Column<DateTime>(nullable: true),
                    Account = table.Column<string>(maxLength: 25, nullable: false),
                    Password = table.Column<string>(nullable: false),
                    Phone = table.Column<string>(nullable: true),
                    UserName = table.Column<string>(nullable: true),
                    LastLogin = table.Column<DateTime>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Picture = table.Column<string>(nullable: true),
                    Salt = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    GroupId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                    table.ForeignKey(
                        name: "FK_User_Group_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Group",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjectImage",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Create = table.Column<string>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()"),
                    Modify = table.Column<string>(nullable: true),
                    ModifyTime = table.Column<DateTime>(nullable: true),
                    ImageName = table.Column<string>(nullable: true),
                    url = table.Column<string>(nullable: true),
                    Rank = table.Column<int>(nullable: false),
                    ProjectId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectImage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectImage_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoleProject",
                columns: table => new
                {
                    RoleId = table.Column<int>(nullable: false),
                    ProjectId = table.Column<int>(nullable: false),
                    Create = table.Column<string>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()"),
                    Operate = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleProject", x => new { x.RoleId, x.ProjectId });
                    table.ForeignKey(
                        name: "FK_RoleProject_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoleProject_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Device",
                columns: table => new
                {
                    DeviceSn = table.Column<string>(nullable: false),
                    Create = table.Column<string>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()"),
                    Modify = table.Column<string>(nullable: true),
                    ModifyTime = table.Column<DateTime>(nullable: true),
                    DeviceName = table.Column<string>(nullable: true),
                    ProductTime = table.Column<DateTime>(nullable: false),
                    UseTime = table.Column<DateTime>(nullable: false),
                    DeviceDescription = table.Column<string>(nullable: true),
                    FullId = table.Column<string>(nullable: true),
                    FullName = table.Column<string>(nullable: true),
                    DeviceNo = table.Column<string>(nullable: true),
                    Position = table.Column<string>(nullable: true),
                    GroupId = table.Column<string>(nullable: true),
                    ProjectId = table.Column<int>(nullable: true),
                    TypeId = table.Column<int>(nullable: false),
                    RegionId = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Device", x => x.DeviceSn);
                    table.ForeignKey(
                        name: "FK_Device_Group_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Group",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Device_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Device_Type_TypeId",
                        column: x => x.TypeId,
                        principalTable: "Type",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TypeAccessory",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Create = table.Column<string>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()"),
                    Modify = table.Column<string>(nullable: true),
                    ModifyTime = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    ICON = table.Column<string>(nullable: true),
                    TypeId = table.Column<int>(nullable: false)
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
                name: "TypeConfig",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Create = table.Column<string>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()"),
                    Modify = table.Column<string>(nullable: true),
                    ModifyTime = table.Column<DateTime>(nullable: true),
                    DataName = table.Column<string>(nullable: true),
                    DataType = table.Column<string>(nullable: true),
                    DataValue = table.Column<string>(nullable: true),
                    TypeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeConfig", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TypeConfig_Type_TypeId",
                        column: x => x.TypeId,
                        principalTable: "Type",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TypeDataDefine",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Create = table.Column<string>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()"),
                    Modify = table.Column<string>(nullable: true),
                    ModifyTime = table.Column<DateTime>(nullable: true),
                    DataKey = table.Column<string>(nullable: true),
                    DataName = table.Column<string>(nullable: true),
                    Unit = table.Column<string>(nullable: true),
                    DataType = table.Column<string>(nullable: true),
                    DefaultValue = table.Column<string>(nullable: true),
                    Format = table.Column<string>(nullable: true),
                    AutoControl = table.Column<bool>(nullable: false),
                    OutKey = table.Column<string>(nullable: true),
                    Model = table.Column<int>(nullable: false, defaultValue: 2),
                    TypeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeDataDefine", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TypeDataDefine_Type_TypeId",
                        column: x => x.TypeId,
                        principalTable: "Type",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TypeHardwareConfig",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Create = table.Column<string>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()"),
                    Modify = table.Column<string>(nullable: true),
                    ModifyTime = table.Column<DateTime>(nullable: true),
                    No = table.Column<int>(nullable: false),
                    Sn = table.Column<int>(nullable: false),
                    Key = table.Column<string>(nullable: true),
                    KeyName = table.Column<string>(nullable: true),
                    ShowKey = table.Column<string>(nullable: true),
                    KeyType = table.Column<string>(nullable: true),
                    Max = table.Column<string>(nullable: true),
                    Min = table.Column<string>(nullable: true),
                    Unit = table.Column<string>(nullable: true),
                    Format = table.Column<string>(nullable: true),
                    Comm = table.Column<string>(nullable: true),
                    Port = table.Column<int>(nullable: true),
                    ModbusSlave = table.Column<string>(nullable: true),
                    PLCType = table.Column<string>(nullable: true),
                    RegAd = table.Column<string>(nullable: true),
                    CMD = table.Column<int>(nullable: false),
                    Address = table.Column<int>(nullable: false),
                    BitOffSet = table.Column<int>(nullable: true),
                    Lens = table.Column<int>(nullable: false),
                    TypeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeHardwareConfig", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TypeHardwareConfig_Type_TypeId",
                        column: x => x.TypeId,
                        principalTable: "Type",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TypeImage",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Create = table.Column<string>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()"),
                    Modify = table.Column<string>(nullable: true),
                    ModifyTime = table.Column<DateTime>(nullable: true),
                    ImageName = table.Column<string>(nullable: true),
                    Url = table.Column<string>(nullable: true),
                    Rank = table.Column<int>(nullable: false, defaultValue: 1),
                    Description = table.Column<string>(nullable: true),
                    TypeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeImage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TypeImage_Type_TypeId",
                        column: x => x.TypeId,
                        principalTable: "Type",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TypeSchema",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Create = table.Column<string>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()"),
                    Modify = table.Column<string>(nullable: true),
                    ModifyTime = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Key = table.Column<string>(nullable: true),
                    Value = table.Column<int>(nullable: false),
                    ParentId = table.Column<int>(nullable: true),
                    TypeId = table.Column<int>(nullable: false),
                    SchemaType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeSchema", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TypeSchema_TypeSchema_ParentId",
                        column: x => x.ParentId,
                        principalTable: "TypeSchema",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TypeSchema_Type_TypeId",
                        column: x => x.TypeId,
                        principalTable: "Type",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TypeStatisticsInfo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Create = table.Column<string>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()"),
                    Modify = table.Column<string>(nullable: true),
                    ModifyTime = table.Column<DateTime>(nullable: true),
                    DataKey = table.Column<string>(nullable: true),
                    StaticsType = table.Column<int>(nullable: false),
                    Standard = table.Column<string>(nullable: true),
                    SUnit = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    DisplayType = table.Column<int>(nullable: false, defaultValue: 0),
                    ShowState = table.Column<int>(nullable: false, defaultValue: 0),
                    Filter = table.Column<string>(nullable: true),
                    FilterType = table.Column<int>(nullable: false, defaultValue: 0),
                    Description = table.Column<string>(nullable: true),
                    TypeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeStatisticsInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TypeStatisticsInfo_Type_TypeId",
                        column: x => x.TypeId,
                        principalTable: "Type",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TypeSystem",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Create = table.Column<string>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()"),
                    Modify = table.Column<string>(nullable: true),
                    ModifyTime = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Order = table.Column<int>(nullable: false),
                    TypeId = table.Column<int>(nullable: false)
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
                name: "TypeUpdateFile",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Create = table.Column<string>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()"),
                    Modify = table.Column<string>(nullable: true),
                    ModifyTime = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Url = table.Column<string>(nullable: true),
                    Version = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    TypeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeUpdateFile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TypeUpdateFile_Type_TypeId",
                        column: x => x.TypeId,
                        principalTable: "Type",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserDepartment",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false),
                    DeparmentId = table.Column<int>(nullable: false),
                    Create = table.Column<string>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDepartment", x => new { x.UserId, x.DeparmentId });
                    table.ForeignKey(
                        name: "FK_UserDepartment_Department_DeparmentId",
                        column: x => x.DeparmentId,
                        principalTable: "Department",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserDepartment_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserRole",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false),
                    RoleId = table.Column<int>(nullable: false),
                    Create = table.Column<string>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRole", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRole_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRole_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DeviceCard",
                columns: table => new
                {
                    CardNo = table.Column<string>(nullable: false),
                    Create = table.Column<string>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()"),
                    Modify = table.Column<string>(nullable: true),
                    ModifyTime = table.Column<DateTime>(nullable: true),
                    AppId = table.Column<string>(nullable: true),
                    AppSecret = table.Column<string>(nullable: true),
                    ExpireTime = table.Column<DateTime>(nullable: true),
                    DeviceSn = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceCard", x => x.CardNo);
                    table.ForeignKey(
                        name: "FK_DeviceCard_Device_DeviceSn",
                        column: x => x.DeviceSn,
                        principalTable: "Device",
                        principalColumn: "DeviceSn",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DeviceConfig",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Create = table.Column<string>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()"),
                    Modify = table.Column<string>(nullable: true),
                    ModifyTime = table.Column<DateTime>(nullable: true),
                    DataName = table.Column<string>(nullable: true),
                    DataType = table.Column<string>(nullable: true),
                    DataValue = table.Column<string>(nullable: true),
                    DeviceSn = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceConfig", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeviceConfig_Device_DeviceSn",
                        column: x => x.DeviceSn,
                        principalTable: "Device",
                        principalColumn: "DeviceSn",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DeviceHardwareConfig",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Create = table.Column<string>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()"),
                    Modify = table.Column<string>(nullable: true),
                    ModifyTime = table.Column<DateTime>(nullable: true),
                    No = table.Column<int>(nullable: false),
                    Sn = table.Column<int>(nullable: false),
                    Key = table.Column<string>(nullable: true),
                    KeyName = table.Column<string>(nullable: true),
                    ShowKey = table.Column<string>(nullable: true),
                    KeyType = table.Column<string>(nullable: true),
                    Max = table.Column<string>(nullable: true),
                    Min = table.Column<string>(nullable: true),
                    Unit = table.Column<string>(nullable: true),
                    Format = table.Column<string>(nullable: true),
                    Comm = table.Column<string>(nullable: true),
                    Port = table.Column<int>(nullable: true),
                    ModbusSlave = table.Column<string>(nullable: true),
                    PLCType = table.Column<string>(nullable: true),
                    RegAd = table.Column<string>(nullable: true),
                    CMD = table.Column<int>(nullable: false),
                    Address = table.Column<int>(nullable: false),
                    BitOffSet = table.Column<int>(nullable: true),
                    Lens = table.Column<int>(nullable: false),
                    DeviceSn = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceHardwareConfig", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeviceHardwareConfig_Device_DeviceSn",
                        column: x => x.DeviceSn,
                        principalTable: "Device",
                        principalColumn: "DeviceSn",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DeviceHisData",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Create = table.Column<string>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()"),
                    Modify = table.Column<string>(nullable: true),
                    ModifyTime = table.Column<DateTime>(nullable: true),
                    DeviceSn = table.Column<string>(nullable: true),
                    Dt = table.Column<DateTime>(nullable: false),
                    DataContent = table.Column<string>(nullable: true),
                    DataTitle = table.Column<string>(nullable: true),
                    GroupId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceHisData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeviceHisData_Device_DeviceSn",
                        column: x => x.DeviceSn,
                        principalTable: "Device",
                        principalColumn: "DeviceSn",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DeviceHisData_Group_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Group",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DeviceImage",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Create = table.Column<string>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()"),
                    Modify = table.Column<string>(nullable: true),
                    ModifyTime = table.Column<DateTime>(nullable: true),
                    ImageName = table.Column<string>(nullable: true),
                    url = table.Column<string>(nullable: true),
                    Rank = table.Column<int>(nullable: false),
                    DeviceSn = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceImage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeviceImage_Device_DeviceSn",
                        column: x => x.DeviceSn,
                        principalTable: "Device",
                        principalColumn: "DeviceSn",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DeviceInputData",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Create = table.Column<string>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()"),
                    Modify = table.Column<string>(nullable: true),
                    ModifyTime = table.Column<DateTime>(nullable: true),
                    DeviceSn = table.Column<string>(nullable: true),
                    Dt = table.Column<DateTime>(nullable: false),
                    WaterInflow = table.Column<int>(nullable: false),
                    COD_In = table.Column<string>(nullable: true),
                    COD_Out = table.Column<string>(nullable: true),
                    COD_Unit = table.Column<string>(nullable: true),
                    NH3_N_In = table.Column<string>(nullable: true),
                    NH3_N_Out = table.Column<string>(nullable: true),
                    NH3_N_Unit = table.Column<string>(nullable: true),
                    TN_In = table.Column<string>(nullable: true),
                    TN_Out = table.Column<string>(nullable: true),
                    TN_Unit = table.Column<string>(nullable: true),
                    TP_In = table.Column<string>(nullable: true),
                    TP_Out = table.Column<string>(nullable: true),
                    TP_Unit = table.Column<string>(nullable: true),
                    SS_In = table.Column<string>(nullable: true),
                    SS_Out = table.Column<string>(nullable: true),
                    SS_Unit = table.Column<string>(nullable: true),
                    PH_In = table.Column<string>(nullable: true),
                    PH_Out = table.Column<string>(nullable: true),
                    PH_Unit = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceInputData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeviceInputData_Device_DeviceSn",
                        column: x => x.DeviceSn,
                        principalTable: "Device",
                        principalColumn: "DeviceSn",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DeviceLog",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Create = table.Column<string>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()"),
                    Modify = table.Column<string>(nullable: true),
                    ModifyTime = table.Column<DateTime>(nullable: true),
                    Account = table.Column<string>(nullable: true),
                    SendTime = table.Column<DateTime>(nullable: false),
                    Time = table.Column<DateTime>(nullable: false),
                    Key = table.Column<string>(nullable: true),
                    KeyName = table.Column<string>(nullable: true),
                    Value = table.Column<string>(nullable: true),
                    OldValue = table.Column<string>(nullable: true),
                    NewValue = table.Column<string>(nullable: true),
                    DeviceSn = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeviceLog_Device_DeviceSn",
                        column: x => x.DeviceSn,
                        principalTable: "Device",
                        principalColumn: "DeviceSn",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DeviceMigration",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Create = table.Column<string>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()"),
                    Modify = table.Column<string>(nullable: true),
                    ModifyTime = table.Column<DateTime>(nullable: true),
                    Account = table.Column<string>(nullable: true),
                    Token = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    CurrentPId = table.Column<int>(nullable: true),
                    PrePId = table.Column<int>(nullable: true),
                    TypeId = table.Column<int>(nullable: false),
                    DeviceSn = table.Column<string>(nullable: true),
                    DeviceNo = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceMigration", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeviceMigration_Device_DeviceSn",
                        column: x => x.DeviceSn,
                        principalTable: "Device",
                        principalColumn: "DeviceSn",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DeviceOnline",
                columns: table => new
                {
                    DeviceSn = table.Column<string>(nullable: false),
                    DeviceNo = table.Column<string>(nullable: true),
                    dt = table.Column<DateTime>(nullable: false),
                    OffLine = table.Column<DateTime>(nullable: true),
                    DataContent = table.Column<string>(nullable: true),
                    DataTitle = table.Column<string>(nullable: true),
                    State = table.Column<bool>(nullable: false),
                    GroupId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceOnline", x => x.DeviceSn);
                    table.ForeignKey(
                        name: "FK_DeviceOnline_Device_DeviceSn",
                        column: x => x.DeviceSn,
                        principalTable: "Device",
                        principalColumn: "DeviceSn",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DeviceOnline_Group_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Group",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DeviceStatisticsData",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Create = table.Column<string>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()"),
                    Modify = table.Column<string>(nullable: true),
                    ModifyTime = table.Column<DateTime>(nullable: true),
                    DeviceSn = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    Data = table.Column<string>(nullable: true),
                    ProjectId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceStatisticsData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeviceStatisticsData_Device_DeviceSn",
                        column: x => x.DeviceSn,
                        principalTable: "Device",
                        principalColumn: "DeviceSn",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DeviceVideo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Create = table.Column<string>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()"),
                    Modify = table.Column<string>(nullable: true),
                    ModifyTime = table.Column<DateTime>(nullable: true),
                    VideoName = table.Column<string>(nullable: true),
                    Url = table.Column<string>(nullable: true),
                    VideoSn = table.Column<string>(nullable: true),
                    Channel = table.Column<int>(nullable: false),
                    SecurityCode = table.Column<string>(nullable: true),
                    DeviceSn = table.Column<string>(nullable: true),
                    Appkey = table.Column<string>(nullable: true),
                    Secret = table.Column<string>(nullable: true),
                    AccessToken = table.Column<string>(nullable: true),
                    ExpireTime = table.Column<long>(nullable: true),
                    ApiUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceVideo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeviceVideo_Device_DeviceSn",
                        column: x => x.DeviceSn,
                        principalTable: "Device",
                        principalColumn: "DeviceSn",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TypeAccessoryControlData",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Create = table.Column<string>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()"),
                    Modify = table.Column<string>(nullable: true),
                    ModifyTime = table.Column<DateTime>(nullable: true),
                    ControlName = table.Column<string>(nullable: true),
                    DataValue = table.Column<string>(nullable: true),
                    DataDefineId = table.Column<int>(nullable: false),
                    AssociateDefineId = table.Column<int>(nullable: false),
                    AccessoryId = table.Column<int>(nullable: false),
                    SequenceIn = table.Column<string>(nullable: true),
                    SequenceOut = table.Column<string>(nullable: true),
                    IState = table.Column<int>(nullable: false)
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
                name: "TypeArgument",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Create = table.Column<string>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()"),
                    Modify = table.Column<string>(nullable: true),
                    ModifyTime = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Category = table.Column<string>(nullable: true),
                    DefineId = table.Column<int>(nullable: false),
                    TypeDataDefineId = table.Column<int>(nullable: true),
                    TypeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeArgument", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TypeArgument_TypeDataDefine_TypeDataDefineId",
                        column: x => x.TypeDataDefineId,
                        principalTable: "TypeDataDefine",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TypeArgument_Type_TypeId",
                        column: x => x.TypeId,
                        principalTable: "Type",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TypeSystemAccessory",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Create = table.Column<string>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()"),
                    Modify = table.Column<string>(nullable: true),
                    ModifyTime = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    ICON = table.Column<string>(nullable: true),
                    SystemId = table.Column<int>(nullable: false)
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
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Create = table.Column<string>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()"),
                    Modify = table.Column<string>(nullable: true),
                    ModifyTime = table.Column<DateTime>(nullable: true),
                    ControlName = table.Column<string>(nullable: true),
                    DataValue = table.Column<string>(nullable: true),
                    DataDefineId = table.Column<int>(nullable: false),
                    AssociateDefineId = table.Column<int>(nullable: false),
                    SequenceIn = table.Column<string>(nullable: true),
                    SequenceOut = table.Column<string>(nullable: true),
                    IState = table.Column<int>(nullable: false),
                    AccessoryId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeSystemAccessoryControlData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TypeSystemAccessoryControlData_TypeSystemAccessory_AccessoryId",
                        column: x => x.AccessoryId,
                        principalTable: "TypeSystemAccessory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TypeSystemAccessoryControlData_TypeDataDefine_DataDefineId",
                        column: x => x.DataDefineId,
                        principalTable: "TypeDataDefine",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Department_GroupId",
                table: "Department",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Department_ParentId",
                table: "Department",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Device_GroupId",
                table: "Device",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Device_ProjectId",
                table: "Device",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Device_TypeId",
                table: "Device",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceCard_DeviceSn",
                table: "DeviceCard",
                column: "DeviceSn",
                unique: true,
                filter: "[DeviceSn] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceConfig_DeviceSn",
                table: "DeviceConfig",
                column: "DeviceSn");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceHardwareConfig_DeviceSn",
                table: "DeviceHardwareConfig",
                column: "DeviceSn");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceHisData_DeviceSn",
                table: "DeviceHisData",
                column: "DeviceSn");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceHisData_GroupId",
                table: "DeviceHisData",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceImage_DeviceSn",
                table: "DeviceImage",
                column: "DeviceSn");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceInputData_DeviceSn",
                table: "DeviceInputData",
                column: "DeviceSn");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceLog_DeviceSn",
                table: "DeviceLog",
                column: "DeviceSn");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceMigration_DeviceSn",
                table: "DeviceMigration",
                column: "DeviceSn");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceOnline_GroupId",
                table: "DeviceOnline",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceStatisticsData_DeviceSn",
                table: "DeviceStatisticsData",
                column: "DeviceSn");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceVideo_DeviceSn",
                table: "DeviceVideo",
                column: "DeviceSn");

            migrationBuilder.CreateIndex(
                name: "IX_Project_GroupId",
                table: "Project",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Project_ParentId",
                table: "Project",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectImage_ProjectId",
                table: "ProjectImage",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Region_GroupId",
                table: "Region",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Region_ParentId",
                table: "Region",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Role_GroupId",
                table: "Role",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleProject_ProjectId",
                table: "RoleProject",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Type_GroupId",
                table: "Type",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Type_ParentId",
                table: "Type",
                column: "ParentId");

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
                name: "IX_TypeArgument_TypeDataDefineId",
                table: "TypeArgument",
                column: "TypeDataDefineId");

            migrationBuilder.CreateIndex(
                name: "IX_TypeArgument_TypeId",
                table: "TypeArgument",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TypeConfig_TypeId",
                table: "TypeConfig",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TypeDataDefine_TypeId",
                table: "TypeDataDefine",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TypeHardwareConfig_TypeId",
                table: "TypeHardwareConfig",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TypeImage_TypeId",
                table: "TypeImage",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TypeSchema_ParentId",
                table: "TypeSchema",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_TypeSchema_TypeId",
                table: "TypeSchema",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TypeStatisticsInfo_TypeId",
                table: "TypeStatisticsInfo",
                column: "TypeId");

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

            migrationBuilder.CreateIndex(
                name: "IX_TypeUpdateFile_TypeId",
                table: "TypeUpdateFile",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_User_GroupId",
                table: "User",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_UserDepartment_DeparmentId",
                table: "UserDepartment",
                column: "DeparmentId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_RoleId",
                table: "UserRole",
                column: "RoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppVersion");

            migrationBuilder.DropTable(
                name: "DataDefineLibrary");

            migrationBuilder.DropTable(
                name: "DeviceCard");

            migrationBuilder.DropTable(
                name: "DeviceConfig");

            migrationBuilder.DropTable(
                name: "DeviceHardwareConfig");

            migrationBuilder.DropTable(
                name: "DeviceHisData");

            migrationBuilder.DropTable(
                name: "DeviceImage");

            migrationBuilder.DropTable(
                name: "DeviceInputData");

            migrationBuilder.DropTable(
                name: "DeviceLog");

            migrationBuilder.DropTable(
                name: "DeviceMigration");

            migrationBuilder.DropTable(
                name: "DeviceOnline");

            migrationBuilder.DropTable(
                name: "DeviceStatisticsData");

            migrationBuilder.DropTable(
                name: "DeviceVideo");

            migrationBuilder.DropTable(
                name: "ProjectImage");

            migrationBuilder.DropTable(
                name: "Region");

            migrationBuilder.DropTable(
                name: "RoleProject");

            migrationBuilder.DropTable(
                name: "TypeAccessoryControlData");

            migrationBuilder.DropTable(
                name: "TypeArgument");

            migrationBuilder.DropTable(
                name: "TypeConfig");

            migrationBuilder.DropTable(
                name: "TypeHardwareConfig");

            migrationBuilder.DropTable(
                name: "TypeImage");

            migrationBuilder.DropTable(
                name: "TypeSchema");

            migrationBuilder.DropTable(
                name: "TypeStatisticsInfo");

            migrationBuilder.DropTable(
                name: "TypeSystemAccessoryControlData");

            migrationBuilder.DropTable(
                name: "TypeUpdateFile");

            migrationBuilder.DropTable(
                name: "UserDepartment");

            migrationBuilder.DropTable(
                name: "UserRole");

            migrationBuilder.DropTable(
                name: "Device");

            migrationBuilder.DropTable(
                name: "TypeAccessory");

            migrationBuilder.DropTable(
                name: "TypeSystemAccessory");

            migrationBuilder.DropTable(
                name: "TypeDataDefine");

            migrationBuilder.DropTable(
                name: "Department");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Project");

            migrationBuilder.DropTable(
                name: "TypeSystem");

            migrationBuilder.DropTable(
                name: "Type");

            migrationBuilder.DropTable(
                name: "Group");
        }
    }
}
