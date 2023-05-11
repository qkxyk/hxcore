using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HXCloud.APIV2.MiddleWares
{
    public class ModuleRequirement : IAuthorizationRequirement
    {
        public ModuleRequirement(int moduleId)
        {
            ModuleId = moduleId;
        }

        public int ModuleId { get; set; }
    }
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
