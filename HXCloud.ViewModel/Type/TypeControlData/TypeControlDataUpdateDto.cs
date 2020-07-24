using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HXCloud.ViewModel
{
    public class TypeControlDataUpdateDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "控制数据名称不能为空")]
        [StringLength(50, ErrorMessage = "控制数据名称长度在2个到50个字符之间", MinimumLength = 2)]
        public string ControlName { get; set; }
        [Required(ErrorMessage = "类型数据定义标示不能为空")]
        public int DataDefineId { get; set; }//对应设备数据栏位编号（和设备栏位对应关系为1：N）

        public int AssociateDefineId { get; set; }//2018-10-15 数据控制新增关联数据栏位字段，满足设备设置值和设备当前值的关联
        public string SequenceIn { get; set; }//设备组内坐标
        public string SequenceOut { get; set; }//设备组间坐标

        public int IState { get; set; } = 0;//标示此控制是否在android端的控制项中，1表示在，0表示不在
    }
}
