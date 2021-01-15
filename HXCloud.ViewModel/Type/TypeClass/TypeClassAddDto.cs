using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HXCloud.ViewModel
{
    public class TypeClassAddDto
    {
        [Required(ErrorMessage = "分组名称不能为空")]
        public string Name { get; set; }
        //分组的序号，用于显示分组所在的位置
        public int Rank { get; set; } = 0;
    }
}
