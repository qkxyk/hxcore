using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HXCloud.ViewModel
{
    /// <summary>
    /// 新增加的角色默认都是非管理员角色，可通过修改赋予角色管理员权限
    /// </summary>
    public class RoleAddDto
    {
        [Required(ErrorMessage ="角色名称不能为空")]
        [StringLength(50,ErrorMessage ="角色名称长度在2到50个字符之间",MinimumLength =2)]
        public string Name { get; set; }//角色名称
        public string Description { get; set; }//角色描述
        //public string GroupId { get; set; }//所属部门编号
        public bool IsAdmin { get; set; } = false;
    }
}
