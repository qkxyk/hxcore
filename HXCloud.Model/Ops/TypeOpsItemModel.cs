using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.Model
{
    /// <summary>
    /// 类型运维项目,同一个类型的运维项目Key不能重复，不同类型的可以重复
    /// </summary>
    public class TypeOpsItemModel : BaseModel, IAggregateRoot
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
        /// <summary>
        /// 关联的设备类型数据
        /// </summary>
        public int TypeId { get; set; }
        public virtual TypeModel Type { get; set; }
    }
}
