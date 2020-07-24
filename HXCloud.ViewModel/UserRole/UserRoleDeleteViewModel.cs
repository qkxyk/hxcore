using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HXCloud.ViewModel
{
    public class UserRoleDeleteViewModel
    {
        [Required(ErrorMessage = "必须输入用户的标识")]
        public int UserId { get; set; }
        [Required(ErrorMessage = "必须输入角色标识")]
        public int RoleId { get; set; }
    }
}
