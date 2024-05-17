using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.ViewModel
{
    /// <summary>
    /// 用户的角色信息
    /// </summary>
    public class UserRoleInfo
    {
        public bool IsAdmin { get; set; }
        public List<RoleModuleInfo> Role { get; set; }//用户的角色列表
    }

    /// <summary>
    /// 角色模块数据
    /// </summary>
    public class RoleModuleInfo
    {
        public int RoleId { get; set; }
        public int ModuleId { get; set; }
    }
}
