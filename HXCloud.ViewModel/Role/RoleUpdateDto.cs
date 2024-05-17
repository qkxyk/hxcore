using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HXCloud.ViewModel
{
    public class RoleUpdateDto
    {
        [Required(ErrorMessage ="角色标示不能为空")]
        public int Id { get; set; }
        [Required(ErrorMessage = "角色名称不能为空")]
        [StringLength(50, ErrorMessage = "角色名称长度在2到50个字符之间", MinimumLength = 2)]
        public string Name { get; set; }//角色名称
        public string Description { get; set; }//角色描述
        public bool IsAdmin { get; set; } = false;
        public int? ModuleId { get; set; }
    }
}
