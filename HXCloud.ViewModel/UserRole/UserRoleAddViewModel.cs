using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HXCloud.ViewModel
{
    public class UserRoleAddViewModel
    {
        [Required(ErrorMessage = "必须输入用户的标识")]
        public int UserId { get; set; }
        [Required(ErrorMessage = "必须输入角色标识")]
        public List<int> RoleId { get; set; }
        [Required(ErrorMessage = "组织标示不能为空")]
        public string GroupId { get; set; }//用于验证用户和角色Id是否匹配
    }
}
