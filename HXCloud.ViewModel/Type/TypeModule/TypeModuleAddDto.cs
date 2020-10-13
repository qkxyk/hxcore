using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HXCloud.ViewModel
{
    public class TypeModuleAddDto
    {
        [Required(ErrorMessage = "模块名称不能为空")]
        [StringLength(25, ErrorMessage = "模块名称在2到25个字符之间", MinimumLength = 2)]
        public string ModuleName { get; set; }
        [Range(1, 2, ErrorMessage = "模块类型只能输入1或者2")]
        public int ModuleType { get; set; } = 2;
        public int Sn { get; set; } = 0;//模块的先后顺序，主要用来排序从模块的先后顺序
        //[Required(ErrorMessage = "类型名称不能为空")]
        //public int TypeId { get; set; }
    }
}
