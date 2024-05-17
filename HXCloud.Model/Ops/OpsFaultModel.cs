using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.Model
{
    /// <summary>
    /// 运维故障表
    /// </summary>
   public class OpsFaultModel : BaseModel, IAggregateRoot
    {
        //public int Id { get; set; }
        public string Code { get; set; }//故障代码
        public string Description { get; set; }//故障描述
        public string Solution { get; set; }//故障解决方法
        public int OpsFaultTypeId { get; set; }//故障类型标识
        public OpsFaultTypeModel OpsFaultType { get; set; }//故障类型
    }

    /// <summary>
    /// 运维故障类型表
    /// </summary>
    public class OpsFaultTypeModel : BaseModel, IAggregateRoot
    {
        public int FaultTypeId { get; set; }//运维故障类型编号
        public int Flag { get; set; } = 1;//用于表示是否是父节点
        public string FaultTypeName { get; set; }//运维故障类型名称
        public int? ParentId { get; set; }//父节点编号
        public OpsFaultTypeModel? Parent { get; set; }
        public ICollection<OpsFaultTypeModel>   Child { get; set; }
        public ICollection<OpsFaultModel> OpsFalt { get; set; }//运维故障表
    }
}
