using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HXCloud.ViewModel
{
    public class DataDefineLibraryAddDto
    {
        [Required(ErrorMessage = "数据定义Key不能为空")]
        public string DataKey { get; set; }//数据标示
        //public string DisplayKey { get; set; }//2018-11-26添加，用于满足设置值和显示值不同，显示设置的结果，此值不做验证。如plc设置为z001，设置后反馈为z002
        [Required(ErrorMessage = "数据定义名称不能为空")]
        public string DataName { get; set; }//数据名称
        public string Unit { get; set; }//单位
        public string DataType { get; set; }//数据类型
        public string DefaultValue { get; set; }//默认值
        #region 数据格式字段，用于数据控制代表不同的含义
        public string Format { get; set; }
        #endregion
        [Range(1, 2, ErrorMessage = "模式只能1或者2")]
        public int Model { get; set; }//是否可读可写，1表示可读，2表示可读可写
    }
}
