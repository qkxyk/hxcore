using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HXCloud.ViewModel
{
    public class TypeModuleControlAddDto
    {
        [Required(ErrorMessage = "控制项名称不能为空")]
        [StringLength(25, ErrorMessage = "控制项名称在2到25个字符之间", MinimumLength = 2)]
        public string ControlName { get; set; }
        public string DataValue { get; set; }//设备设置值
        public string Formula { get; set; }   //公式，用于反馈项的设置
        public int Sn { get; set; } = 0;//显示顺序
        [Required(ErrorMessage = "关联的类型数据定义标示不能为空")]
        public int DataDefineId { get; set; }//对应设备数据栏位编号（和设备栏位对应关系为1：N）
        [Required(ErrorMessage = "模块标示不能为空")]
        public int ModuleId { get; set; }//模块标示
    }
}
