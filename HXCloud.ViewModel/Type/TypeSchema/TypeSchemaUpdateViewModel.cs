using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HXCloud.ViewModel
{
    public class TypeSchemaUpdateViewModel
    {
        [Required(ErrorMessage = "模式标示不能为空")]
        public int Id { get; set; }
        [Required(ErrorMessage = "模式名称不能为空")]
        [StringLength(20, ErrorMessage = "模式名称长度在2个字符和20个字符之间", MinimumLength = 2)]
        public string Name { get; set; }//模式名称
    }
}
