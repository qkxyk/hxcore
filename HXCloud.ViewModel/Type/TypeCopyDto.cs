using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HXCloud.ViewModel
{
    public class TypeCopyDto
    {
        [Required(ErrorMessage ="源类型标示不能为空")]
        public int SourceId { get; set; }
        [Required(ErrorMessage ="目标类型标示不能为空")]
        public int TargetId { get; set; }
    }
}
