using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HXCloud.ViewModel
{
 public   class RoleModuleOperateAddDto
    {
        [Required]
        public int RoleId { get; set; }//角色标识
        [Required]
        public int OperateId { get; set; }//模块操作标识
    }
    public class RoleModuleOperateDto
    {
        public int RoleId { get; set; }//角色标识
        public int OperateId { get; set; }//模块操作标识
    }
}
