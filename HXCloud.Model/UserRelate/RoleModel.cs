using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.Model
{
    public class RoleModel : BaseModel, IAggregateRoot
    {
        public int Id { get; set; } //角色标示
        public string RoleName { get; set; }//角色名称
        public string Description { get; set; }//角色描述
        //public DateTime CreateTime { get; set; }//创建时间
        public bool IsAdmin { get; set; } = false;//是否管理员
        //public int DepartmentId { get; set; }//所属部门编号
        //public virtual DepartmentModel Department { get; set; }//所属部门信息
        public string GroupId { get; set; }
        public GroupModel Group { get; set; }
        public virtual ICollection<UserRoleModel> UserRoles { get; set; }//角色用户
        //public virtual ICollection<RoleMenuModel> RoleMenus { get; set; }//角色菜单权限
        public virtual ICollection<RoleProjectModel> RoleProjects { get; set; }//角色项目权限

        public int? ModuleId { get; set; }
        public virtual ModuleModel Module { get; set; }
        public ICollection<RoleModuleOperateModel> RoleModuleOperates { get; set; }//角色模块的权限
    }
}
