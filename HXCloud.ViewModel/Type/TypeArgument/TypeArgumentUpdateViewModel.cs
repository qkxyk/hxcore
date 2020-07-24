using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HXCloud.ViewModel
{
    public class TypeArgumentUpdateViewModel
    {
        [Required(ErrorMessage = "参数标示不能为空")]
        public int Id { get; set; }
        [Required(ErrorMessage = "参数名称不能为空")]
        [StringLength(50, ErrorMessage = "参数名称长度为2到50个字符之间", MinimumLength = 2)]
        public string Name { get; set; }
        public string Category { get; set; }//默认的一些配置数据(约定数据)
        [Required(ErrorMessage = "必须输入关联的数据定义的标示")]
        public int DefineId { get; set; }
    }
}
