using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.ViewModel
{
    public class RoleViewModel
    {
        public int Id { get; set; } //角色标示
        public string Name { get; set; }//角色名称
        public string Description { get; set; }//角色描述
        public DateTime CreateTime { get; set; }//创建时间
        //public bool IsAdmin { get; set; } = false;//是否管理员
        public int DepartmentId { get; set; }//所属部门编号
    }
}
