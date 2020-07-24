using System;
using System.Collections.Generic;
using System.Text;
using HXCloud.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using HXCloud.Model;
using HXCloud.Common;

//#Debug

namespace HXCloud.Service
{
    //数据库种子文件，因要引用Model数据，所以种子文件放在service项目中
    public static class SeedData
    {
        static string groupid = new Guid("057a556af24f494094a2586fbe9335a4").ToString("N");
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new HXCloudContext(serviceProvider.GetRequiredService<DbContextOptions<HXCloudContext>>()))
            {
                if (context.Groups.Any())
                {
                    return;
                }
                //添加种子文件
                #region 示例代码
                /*
                context.Movie.AddRange(
                    new Movie
                    {
                        Title = "When Harry Met Sally",
                        ReleaseDate = DateTime.Parse("1989-2-12"),
                        Genre = "Romantic Comedy",
                        Price = 7.99M
                    },
                    new Movie
                    {
                        Title = "Ghostbusters ",
                        ReleaseDate = DateTime.Parse("1984-3-13"),
                        Genre = "Comedy",
                        Price = 8.99M
                    },

                    new Movie
                    {
                        Title = "Ghostbusters 2",
                        ReleaseDate = DateTime.Parse("1986-2-23"),
                        Genre = "Comedy",
                        Price = 9.99M
                    },

                    new Movie
                    {
                        Title = "Rio Bravo",
                        ReleaseDate = DateTime.Parse("1959-4-15"),
                        Genre = "Western",
                        Price = 3.99M
                    }
                    );
                */
                #endregion
                #region 添加组织
                context.Groups.Add(new GroupModel() { GroupCode = "hx", GroupName = "合续", Id = groupid, Create = "test" });
                #endregion
                #region 添加角色
                var role = new RoleModel
                {
                    //Id = 1,
                    RoleName = "Admin",
                    GroupId = groupid,
                    Description = "租户管理员",
                    IsAdmin = true
                };
                context.Roles.Add(role);
                #endregion
                #region 添加部门
                var dParent = new DepartmentModel
                {
                    DepartmentName = "合续环境",
                    GroupId = groupid,
                    Description = "合续环境公司",
                    Level = 1,
                    DepartmentType = DepartmentType.Normal
                };
                var dChild = new DepartmentModel
                {
                    DepartmentName = "岗位",
                    GroupId = groupid,
                    Description = "合续环境公司",
                    Level = 2,
                    DepartmentType = DepartmentType.Station,
                    PathId = dParent.Id.ToString(),
                    PathName = "合续环境",
                    Parent = dParent
                };
                context.Departments.AddRange(dParent, dChild);
                #endregion
                #region 添加主管理员
                string salt = EncryptData.CreateRandom();
                string password = EncryptData.EncryptPassword("hx123456", salt);
                var user = new UserModel
                {
                    Account = "hx0001",
                    Password = password,
                    GroupId = groupid,
                    Status = HXCloud.Model.UserStatus.Valid,
                    UserName = "hx001",
                    Salt = salt,
                    Picture = "/Images/default.jpg"
                    //Id = 1
                };
                context.Users.Add(user);
                context.UserRoles.Add(new UserRoleModel { User = user, Role = role });
                context.UserDepartments.Add(new UserDepartmentModel { User = user, Department = dChild });
                #endregion


#if DEBUG
                #region 添加默认数据定义
                context.DataDefineLibray.AddRange(
                    new DataDefineLibraryModel
                    {
                        //Id = 1,
                        DataKey = "011241",
                        DataName = "污水提升泵1#停止时间当前值",
                        Unit = "[\"Time\",\"m\"]",
                        DataType = "string",
                        DefaultValue = "",
                        Format = "{\"step\":1}",
                        Model = 0,
                        Create = "hx0001",
                        CreateTime = DateTime.Now
                    },
                    new DataDefineLibraryModel
                    {
                        //Id = 2,
                        DataKey = "011111",
                        DataName = "污水提升泵1#手动启动",
                        Unit = "[\" - \"]",
                        DataType = "boolean",
                        DefaultValue = "",
                        Format = "{\"0\":0,\"1\":1}",
                        Model = 0,
                        Create = "hx0001",
                        CreateTime = DateTime.Now
                    },
                    new DataDefineLibraryModel
                    {
                        //Id = 3,
                        DataKey = "011221",
                        DataName = "污水提升泵1#运行时间当前值",
                        Unit = "[\"Time\",\"m\"]",
                        DataType = "string",
                        DefaultValue = "",
                        Format = "{\"step\":1}",
                        Model = 0,
                        Create = "hx0001",
                        CreateTime = DateTime.Now
                    },
                    new DataDefineLibraryModel
                    {
                        //Id = 4,
                        DataKey = "021521",
                        DataName = "污水提升泵1#变频器频率当前值",
                        Unit = "[\"frequency\",\"HZ\"]",
                        DataType = "string",
                        DefaultValue = "",
                        Format = "{\"step\":100}",
                        Model = 0,
                        Create = "hx0001",
                        CreateTime = DateTime.Now
                    }
                );
                #endregion
#endif

                context.SaveChanges();
            }
        }
    }
}
