using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.Model
{
    /// <summary>
    /// 维修数据
    /// </summary>
    public class RepairModel : BaseModel, IAggregateRoot
    {
        public string Id { get; set; }
        public string DeviceSn { get; set; }
        public string DeviceName { get; set; }
        /// <summary>
        /// 关联的问题单编号
        /// </summary>
        public int IssueId { get; set; }
        public RepairType RepairType { get; set; }
        public RepairStatus RepairStatus { get; set; }
        ///// <summary>
        ///// 接单人
        ///// </summary>      
        public string Receiver { get; set; }
        /// <summary>
        /// 接单时间
        /// </summary>
        public DateTime? ReceiveTime { get; set; }
        /// <summary>
        /// 接单人电话
        /// </summary>
        public string ReceivePhone { get; set; }
        public EmergenceStatus EmergenceStatus { get; set; }
        public string Url { get; set; }
        /// <summary>
        /// 完成时间
        /// </summary>
        public DateTime CompleteTime { get; set; }
        public DateTime? WaitTime { get; set; }//配件等待开始时间
        public DateTime? CheckTime { get; set; }//核验时间
        public string CheckDescription { get; set; }//核验描述
        public string Description { get; set; }//提交描述
        public string WaitDescription { get; set; }//等待配件描述

    }
    /// <summary>
    /// 维修类型，维修或者调试
    /// </summary>
    public enum RepairType
    {
        Repair, Debug
    }
    /// <summary>
    /// 维修或者调试单状态,派单，维修在途，等待配件，等待审核，完成，接单是操作，把单据的状态更改为维修或者调试在途
    /// </summary>
    public enum RepairStatus
    {
        Send, Way, Wait, Check, Complete
    }
    /// <summary>
    /// 工单紧急情况，一般，紧急，特急
    /// </summary>
    public enum EmergenceStatus
    {
        Normal, Emergence, Extra
    }
}
