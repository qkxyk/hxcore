using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HXCloud.APIV2.MiddleWares
{
    public class ModuleRequirement : IAuthorizationRequirement
    {
        public ModuleRequirement(int moduleId,string code,List<int> roles)
        {
            ModuleId = moduleId;
            Code = code;
            Roles = roles;
        }

        public int ModuleId { get; set; }//模块
        public string Code { get; set; }//验证的操作
        public List<int> Roles { get; set; }//模块权限的角色(验证权限时传入的角色列表)
        public List<int> ModuleRoles { get; set; } = new List<int>();//用户有模块权限的角色(返回，用于根据角色获取用户的项目)
    }
    /// <summary>
    /// 角色项目权限验证，目前暂未使用
    /// </summary>
    public class ResourceData
    {
        /// <summary>
        /// 项目或者场站标识
        /// </summary>
        public int ProjectId { get; set; }
        /// <summary>
        /// 验证的操作
        /// </summary>
        public int Operate { get; set; }
        /// <summary>
        /// 验证权限如何比较
        /// </summary>
        public CompareData Compare { get; set; }
    }
    public enum CompareData
    {
        Equal, Great, GreatEqual
    }

}
