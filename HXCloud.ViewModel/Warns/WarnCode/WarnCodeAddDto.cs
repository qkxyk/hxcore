using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HXCloud.ViewModel
{
    public class WarnCodeAddDto
    {
        public string Description { get; set; }//报警描述
        [Required(ErrorMessage = "编码不能为空")]
        [StringLength(50, ErrorMessage = "编码长度在4到50个字符之间", MinimumLength = 4)]
        public string Code { get; set; }//报警编码,报警、故障或者通知编码，唯一
    }
}
