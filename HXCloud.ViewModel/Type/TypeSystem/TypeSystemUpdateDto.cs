using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HXCloud.ViewModel
{
    public class TypeSystemUpdateDto
    {
        [Required(ErrorMessage = "子系统标示不能为空")]
        public int Id { get; set; }
        [Required(ErrorMessage = "子系统名称不能为空")]
        [StringLength(50, ErrorMessage = "子系统名称长度在2到50个字符之间", MinimumLength = 2)]
        public string Name { get; set; }
        public int Order { get; set; } = 1;//备用，用于子系统的排名

    }
}
