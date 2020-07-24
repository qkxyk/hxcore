using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HXCloud.ViewModel
{
    public class TypeSchemaAddViewModel
    {
        [Required(ErrorMessage = "模式名称不能为空")]
        [StringLength(20, ErrorMessage = "模式名称长度在2个字符和20个字符之间", MinimumLength = 2)]
        public string Name { get; set; }//模式名称
        [Required(ErrorMessage = "类型数据定义标示不能为空")]
        public int DataDefineId { get; set; }
        //[Required(ErrorMessage = "模式对应的Key值不能为空")]
        //public string Key { get; set; }//模式对应的key
        public int Value { get; set; }
        public Nullable<int> ParentId { get; set; }
        //[Required(ErrorMessage = "类型标示不能为空")]
        //public int TypeId { get; set; }
        [Required(ErrorMessage = "模式类型为自动运行和自定义模式")]
        [Range(0, 1, ErrorMessage = "模式类型只能为0和1")]
        public int SchemaType { get; set; }
    }
}
