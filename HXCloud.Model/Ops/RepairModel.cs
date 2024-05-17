using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.Model
{
    /// <summary>
    /// 维修工单
    /// 2023-2-14 因要保留工单每一步的数据，将原工单拆分为工单表和工单流程数据表，工单表中添加工单当前状态数据
    /// </summary>
    public class RepairModel : BaseModel, IAggregateRoot
    {
        public string Id { get; set; }
        public string DeviceSn { get; set; }
        public string DeviceName { get; set; }
        public string ProjectName { get; set; }
        /// <summary>
        /// 关联的问题单编号
        /// </summary>
        public int IssueId { get; set; }
        //public IssueModel Issue { get; set; }//关联的问题单
        public RepairType RepairType { get; set; }
        public RepairStatus RepairStatus { get; set; }
        public string CreateName { get; set; }//冗余创建者
        ///// <summary>
        ///// 接单人
        ///// </summary>      
        public string Receiver { get; set; }
        public string ReceiverName { get; set; }//冗余接单人
        ///// <summary>
        ///// 接单时间
        ///// </summary>
        //public DateTime? ReceiveTime { get; set; }
        /// <summary>
        /// 接单人电话
        /// </summary>
        public string ReceivePhone { get; set; }
        public EmergenceStatus EmergenceStatus { get; set; }
        //public string Url { get; set; }
        /// <summary>
        /// 完成时间
        /// </summary>
        public DateTime? CompleteTime { get; set; }
        //public DateTime? WaitTime { get; set; }//配件等待开始时间
        //public DateTime? CheckTime { get; set; }//核验时间
        //public string CheckAccount { get; set; }//核验人
        //public string CheckAccountName { get; set; }//冗余审核人员
        //public string CheckDescription { get; set; }//核验描述
        public string Description { get; set; }//提交描述
        //public string WaitDescription { get; set; }//等待配件描述

        //配件信息
        public bool IsParts { get; set; } = false;//是否需要配件
        public virtual ICollection<RepairPartModel> RepairParts { get; set; }
        //工单流程
        public virtual ICollection<RepairDataModel> RepairDatas { get; set; }

        public bool IsComplete { get; set; } = false;//流程是否结束
        public string FaultCode { get; set; }//故障代码，此功能主要用来统计故障数据和故障代码弱关联

    }
    /// <summary>
    /// 维修类型，维修或者调试
    /// </summary>
    public enum RepairType
    {
        Repair, Debug
    }
    /// <summary>
    /// 工单状态依次为：已派单、运维再途、等待第三方维修、等待配件、等待审核、完成。
    /// 接单之后工单状态修改为运维在途，第三方维修和等待配件是并列状态，运维在途、等待配件和第三方维修状态下可以提交资料。
    /// </summary>
    public enum RepairStatus
    {
        Send, Way, Third, Wait, Check, Complete
    }
    /// <summary>
    /// 工单紧急情况，一般，紧急，特急
    /// </summary>
    public enum EmergenceStatus
    {
        Normal, Emergence, Extra
    }
}
