using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HXCloud.ViewModel
{
    public class RoleProjectAddDto
    {
        [Required(ErrorMessage ="角色标示不能为空")]
        public int RoleId { get; set; }
        [Required(ErrorMessage ="项目标示不能为空")]
        public string ProjectId { get; set; }//项目标示列表，用逗号分割
        [Required(ErrorMessage ="操作标示不能为空")]
        public string Operate { get; set; }//操作标示列表，用逗号分割
    }
}
