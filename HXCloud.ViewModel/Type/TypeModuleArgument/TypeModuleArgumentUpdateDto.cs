using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HXCloud.ViewModel
{
    public class TypeModuleArgumentUpdateDto
    {
        [Required(ErrorMessage = "配置项标示不能为空")]
        public int Id { get; set; }
        [Required(ErrorMessage = "配置数据名称不能为空")]
        public string Name { get; set; }
        [Required(ErrorMessage = "分类标识不能为空")]
        public string Category { get; set; }//用来标识属于什么，如,T2代表手自动，T5代表报警复位
        [Required(ErrorMessage = "关联的类型数据定义标示不能为空")]
        public int DataDefineId { get; set; }//类型数据定义标识
    }
}
