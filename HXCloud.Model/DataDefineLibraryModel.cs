using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.Model
{
    //数据定义库，此库的数据只有主管理员有权限添加、编辑，类型数据定义仅支持在库中进行选择
    public class DataDefineLibraryModel : BaseModel, IAggregateRoot
    {
        public int Id { get; set; }
        public string DataKey { get; set; }//数据标示
        //public string DisplayKey { get; set; }//2018-11-26添加，用于满足设置值和显示值不同，显示设置的结果，此值不做验证。如plc设置为z001，设置后反馈为z002
        public string DataName { get; set; }//数据名称
        public string Unit { get; set; }//单位
        public string DataType { get; set; }//数据类型
        public string DefaultValue { get; set; }//默认值
        #region 数据格式字段，用于数据控制代表不同的含义
        public string Format { get; set; }
        #endregion
        public int Model { get; set; }//是否可读可写
        public string Category { get; set; }//数据定义标签，便于查找
    }
}
