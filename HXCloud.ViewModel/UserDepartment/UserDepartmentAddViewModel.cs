using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HXCloud.ViewModel
{
    public class UserDepartmentAddViewModel
    {
        [Required(ErrorMessage = "必须输入用户的标识")]
        public int UserId { get; set; }
        [Required(ErrorMessage = "必须输入部门标识")]
        public int DepartmentId { get; set; }
    }
}
