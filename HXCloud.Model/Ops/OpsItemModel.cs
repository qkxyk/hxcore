using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.Model
{
    /// <summary>
    /// 运维项目,展示的项目要合并类型的运维项目
    /// </summary>
    public class OpsItemModel:BaseModel,IAggregateRoot
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string Name { get; set; }
        /// <summary>
        /// 数据类型
        /// </summary>
        public string DataType { get; set; }
        /// <summary>
        /// 指示该数据属于那个模块
        /// </summary>
        public OpsType OpsType { get; set; }
        public string Unit { get; set; }
        public float Max { get; set; }
        public float Min { get; set; }

    }

    /// <summary>
    /// 运维类型，product生产数据，water水质分析，device设备巡检，technique工艺巡检
    /// </summary>
    public enum OpsType
    {
        Product,Water,Device,Technique
    }
}
