using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HXCloud.ViewModel
{
    public class TypeDisplayIconAddDto
    {
        [Required(ErrorMessage = "名称不能为空")]
        [StringLength(20, ErrorMessage = "名称长度在2个字符到20个字符之间", MinimumLength = 2)]
        public string Name { get; set; }
        public string Icon { get; set; }
        public int Sn { get; set; } = 0;//排序序号
        [Required(ErrorMessage = "关联的数据定义标示不能为空")]
        public int DataDefineId { get; set; }
    }
}
