using HXCloud.Common;
using HXCloud.Model;
using HXCloud.Repository.Maps;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.Repository
{
    public class HXCloudContext : DbContext
    {
        /*
        public static readonly ILoggerFactory MyLoggerFactory
         = LoggerFactory.Create(builder =>
         {
             builder
            .AddFilter((category, level) =>
                category == DbLoggerCategory.Database.Command.Name
                && level == LogLevel.Information)
            .AddConsole();
         });
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
=> optionsBuilder
   .UseLoggerFactory(MyLoggerFactory); // Warning: Do not create a new ILoggerFactory instance each time
        //.UseSqlServer(
        //    @"Server=(localdb)\mssqllocaldb;Database=EFLogging;Trusted_Connection=True;ConnectRetryCount=0");
        */
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
        }
        public HXCloudContext(DbContextOptions<HXCloudContext> options) : base(options)
        {

        }


        public DbSet<GroupModel> Groups { get; set; }
        public DbSet<AppVersionModel> AppVersions { get; set; }
        public DbSet<RegionModel> Regions { get; set; }

        public DbSet<UserModel> Users { get; set; }
        public DbSet<UserRoleModel> UserRoles { get; set; }
        public DbSet<RoleModel> Roles { get; set; }
        public DbSet<UserDepartmentModel> UserDepartments { get; set; }
        public DbSet<DepartmentModel> Departments { get; set; }
        public DbSet<DataDefineLibraryModel> DataDefineLibray { get; set; }

        public DbSet<TypeModel> Types { get; set; }
        public DbSet<TypeImageModel> TypeImages { get; set; }
        public DbSet<TypeUpdateFileModel> TypeUpdateFiles { get; set; }
        public DbSet<TypeStatisticsInfoModel> TypeStatisticsInfos { get; set; }
        public DbSet<TypeDataDefineModel> TypeDataDefines { get; set; }
        public DbSet<TypeHardwareConfigModel> TypeHardwareConfigs { get; set; }
        #region 2020-12-17删除
        /*
        public DbSet<TypeAccessoryModel> TypeAccessories { get; set; }
        public DbSet<TypeAccessoryControlDataModel> TypeAccessoryControlDatas { get; set; }
        public DbSet<TypeSystemModel> TypeSystems { get; set; }
        public DbSet<TypeSystemAccessoryModel> TypeSystemAccessories { get; set; }
        public DbSet<TypeSystemAccessoryControlDataModel> TypeSystemAccessoryControlDatas { get; set; }
        */
        #endregion
        public DbSet<TypeSchemaModel> TypeSchemas { get; set; }
        public DbSet<TypeConfigModel> TypeConfigs { get; set; }
        public DbSet<TypeArgumentModel> TypeArguments { get; set; }

        public DbSet<TypeOverviewModel> TypeOverviews { get; set; }
        public DbSet<TypeDisplayIconModel> TypeDisplayIcons { get; set; }
        public DbSet<TypeModuleModel> TypeModules { get; set; }
        public DbSet<TypeModuleControlModel> TypeModuleControls { get; set; }
        public DbSet<TypeModuleFeedbackModel> typeModuleFeedbacks { get; set; }


        public DbSet<ProjectModel> Projects { get; set; }
        public DbSet<ProjectImageModel> ProjectImages { get; set; }
        public DbSet<RoleProjectModel> RoleProjects { get; set; }

        public DbSet<DeviceModel> Devices { get; set; }
        public DbSet<DeviceCardModel> DeviceCards { get; set; }
        public DbSet<DeviceStatisticsDataModel> DeviceStatisticsData { get; set; }
        public DbSet<DeviceDiscreteStatisticsDataModel> DeviceDiscreteStatisticsData { get; set; }
        public DbSet<DeviceHisDataModel> DeviceHisData { get; set; }
        public DbSet<DeviceImageModel> DeviceImages { get; set; }
        public DbSet<DeviceVideoModel> DeviceVideos { get; set; }
        public DbSet<DeviceConfigModel> DeviceConfigs { get; set; }
        public DbSet<DeviceHardwareConfigModel> DeviceHardwareConfigs { get; set; }
        public DbSet<DeviceInputDataModel> DeviceInputData { get; set; }
        public DbSet<DeviceMigrationModel> DeviceMigrations { get; set; }
        public DbSet<DeviceOnlineModel> DeviceOnlines { get; set; }
        public DbSet<DeviceLogModel> DeviceLogs { get; set; }

        public DbSet<WarnTypeModel> WarnTypes { get; set; }
        public DbSet<WarnCodeModel> WarnCodes { get; set; }
        public DbSet<WarnModel> Warns { get; set; }

        public DbSet<CategoryModel> Categories { get; set; }



        /*
      public DbSet<MenuModel> Menus { get; set; }
     
      public DbSet<RoleMenuModel> RoleMenus { get; set; }
      */
        string groupid = new Guid("057a556af24f494094a2586fbe9335a4").ToString("N");
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new GroupModelMap());
            modelBuilder.ApplyConfiguration(new AppVersionModelMap());
            modelBuilder.ApplyConfiguration(new RegionModelMap());

            modelBuilder.ApplyConfiguration(new UserModelMap());
            modelBuilder.ApplyConfiguration(new RoleModelMap());
            modelBuilder.ApplyConfiguration(new UserRoleModelMap());
            modelBuilder.ApplyConfiguration(new DepartmentModelMap());
            modelBuilder.ApplyConfiguration(new UserDepartmentModelMap());
            modelBuilder.ApplyConfiguration(new DataDefineLibraryModelMap());

            modelBuilder.ApplyConfiguration(new TypeModelMap());
            modelBuilder.ApplyConfiguration(new TypeImageModelMap());
            modelBuilder.ApplyConfiguration(new TypeUpdateFileModelMap());
            modelBuilder.ApplyConfiguration(new TypeDataDefineModelMap());
            modelBuilder.ApplyConfiguration(new TypeStatisticsInfoModelMap());
            modelBuilder.ApplyConfiguration(new TypeSchemaModelMap());
            modelBuilder.ApplyConfiguration(new TypeConfigModelMap());
            modelBuilder.ApplyConfiguration(new TypeArgumentModelMap());
            modelBuilder.ApplyConfiguration(new TypeHardwareConfigModelMap());
            #region 2020-12-17删除
            /*
            modelBuilder.ApplyConfiguration(new TypeAccessoryModelMap());
            modelBuilder.ApplyConfiguration(new TypeAccessoryControlDataModelMap());
           
            modelBuilder.ApplyConfiguration(new TypeSystemModelMap());
            modelBuilder.ApplyConfiguration(new TypeSystemAccessoyModelMap());
            modelBuilder.ApplyConfiguration(new TypeSystemAccessoryControlDataModelMap());
            */
            #endregion
            modelBuilder.ApplyConfiguration(new TypeOverviewModelMap());
            modelBuilder.ApplyConfiguration(new TypeDisplayIconModelMap());
            modelBuilder.ApplyConfiguration(new TypeModuleModelMap());
            modelBuilder.ApplyConfiguration(new TypeModuleControlModelMap());
            modelBuilder.ApplyConfiguration(new TypeModuleFeedbackModelMap());

            modelBuilder.ApplyConfiguration(new DeviceModelMap());
            modelBuilder.ApplyConfiguration(new DeviceCardModelMap());
            modelBuilder.ApplyConfiguration(new DeviceStatisticsDataModelMap());
            modelBuilder.ApplyConfiguration(new DeviceDiscreteStatisticsDataModelMap());
            modelBuilder.ApplyConfiguration(new DeviceHisDataModelMap());
            modelBuilder.ApplyConfiguration(new DeviceImageModelMap());
            modelBuilder.ApplyConfiguration(new DeviceVideoModelMap());
            modelBuilder.ApplyConfiguration(new DeviceConfigModelMap());
            modelBuilder.ApplyConfiguration(new DeviceHardwareConfigModelMap());
            modelBuilder.ApplyConfiguration(new DeviceInputDataModelMap());
            modelBuilder.ApplyConfiguration(new DeviceMigrationModelMap());
            modelBuilder.ApplyConfiguration(new DeviceOnlineModelMap());
            modelBuilder.ApplyConfiguration(new DeviceLogModelMap());

            modelBuilder.ApplyConfiguration(new ProjectModelMap());
            modelBuilder.ApplyConfiguration(new ProjectImageModelMap());
            modelBuilder.ApplyConfiguration(new RoleProjectModelMap());

            modelBuilder.ApplyConfiguration(new WarnTypeModelMap());
            modelBuilder.ApplyConfiguration(new WarnCodeModelMap());
            modelBuilder.ApplyConfiguration(new WarnModelMap());

            modelBuilder.ApplyConfiguration(new CategoryModelMap());

            #region 初始化种子数据
            /*
            modelBuilder.Entity<GroupModel>().HasData(new GroupModel() { GroupCode = "hx", GroupName = "合续", Id = groupid, Create = "test" });
            modelBuilder.Entity<RoleModel>().HasData(new RoleModel()
            {
                Id = 1,
                RoleName = "Admin",
                GroupId = groupid,
                Description = "租户管理员",
                IsAdmin = true
            });
            string salt = EncryptData.CreateRandom();
            string password = EncryptData.EncryptPassword("hx123456", salt);
            modelBuilder.Entity<UserModel>().HasData(new UserModel()
            {
                Account = "hx0001",
                Password = password,
                GroupId = groupid,
                Status = HXCloud.Model.UserStatus.Valid,
                UserName = "hx001",
                Salt = salt,
                Picture = "/Images/default.jpg",
                Id = 1
            });
            modelBuilder.Entity<UserRoleModel>().HasData(new UserRoleModel()
            {
                UserId = 1,
                RoleId = 1
            });
            modelBuilder.Entity<DepartmentModel>().HasData(new DepartmentModel
            {
                Id = 1,
                DepartmentName = "合续环境",
                GroupId = groupid,
                Description = "合续环境公司",
                Level = 1,
                DepartmentType = DepartmentType.Normal
            });
            modelBuilder.Entity<DepartmentModel>().HasData(new DepartmentModel
            {
                Id = 2,
                DepartmentName = "岗位",
                GroupId = groupid,
                Description = "合续环境公司",
                Level = 2,
                DepartmentType = DepartmentType.Station,
                PathId = "1",
                PathName = "合续环境"
            });
            modelBuilder.Entity<UserDepartmentModel>().HasData(new UserDepartmentModel
            {
                UserId = 1,
                DeparmentId = 2
            });
            */
            #endregion
            /*
            modelBuilder.ApplyConfiguration(new MenuModelMap());
            modelBuilder.ApplyConfiguration(new ProjectModelMap());
            modelBuilder.ApplyConfiguration(new RoleMenuModelMap());
            modelBuilder.ApplyConfiguration(new RoleProjectModelMap());
                       */
        }
    }
}
