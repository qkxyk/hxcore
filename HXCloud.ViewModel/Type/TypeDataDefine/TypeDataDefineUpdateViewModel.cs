using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HXCloud.ViewModel
{
    public class TypeDataDefineUpdateViewModel
    {
        public int Id { get; set; }
        //public string DataKey { get; set; }//数据标示

        public string DataName { get; set; }//数据名称
        public string Unit { get; set; }//单位
        public string DataType { get; set; }//数据类型
        //20202206新增ShowType，showType为平台使用的数据类型,datatype为设备使用的类型
        public string ShowType { get; set; }//数据类型
        public string DefaultValue { get; set; }//默认值
        #region 数据格式字段，用于数据控制代表不同的含义
        public string Format { get; set; }
        #endregion

        public bool AutoControl { get; set; } = false;//自动模式下是否可以控制
        public string OutKey { get; set; }//Display更改为outkey 2018-11-26添加，用于满足设置值和显示值不同，显示设置的结果，此值不做验证。如plc设置为z001，设置后反馈为z002
        [Range(1, 2, ErrorMessage = "模式只能1或者2")]
        public int Model { get; set; }//数据定义默认是可写（后加入字段为兼容默认为是可以写）
        //[Required(ErrorMessage = "类型标示不能为空")]
        //public int TypeId { get; set; }
        public string Category { get; set; }
    }
}
