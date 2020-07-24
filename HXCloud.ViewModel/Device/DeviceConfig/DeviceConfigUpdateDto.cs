using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HXCloud.ViewModel
{
    public class DeviceConfigUpdateDto
    {
        [Required(ErrorMessage = "配置数据标示不能为空")]
        public int Id { get; set; }
        [Required(ErrorMessage = "配置数据名称不能为空")]
        [StringLength(50, ErrorMessage = "配置数据名称长度在2到50个字符之间")]
        public string DataName { get; set; }
        public string DataType { get; set; }//配置类型，使用者定义
        public string DataValue { get; set; }
    }
}
