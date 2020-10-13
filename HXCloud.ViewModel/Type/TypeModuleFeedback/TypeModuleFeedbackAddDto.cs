using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HXCloud.ViewModel
{
    public class TypeModuleFeedbackAddDto
    {
        public int Sn { get; set; } = 0;//处理多个反馈值时序号，可以应对control项的计算公式
        [Required(ErrorMessage = "数据控制标示不能为空")]
        public int ModuleControlId { get; set; }//对应控制项
        [Required(ErrorMessage = "关联的类型数据定义标示不能为空")]
        public int DataDefineId { get; set; }
    }
}
