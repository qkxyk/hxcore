using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HXCloud.ViewModel
{
    public class WarnTypeAddDto
    {
        [Required(ErrorMessage = "类型名称不能为空")]
        [StringLength(50, ErrorMessage = "类型名称长度在2到50个字符之间", MinimumLength = 2)]
        public string TypeName { get; set; }

        public string Color { get; set; }
        public string Icon { get; set; }
    }
}
