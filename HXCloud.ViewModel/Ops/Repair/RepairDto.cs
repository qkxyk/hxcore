using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HXCloud.ViewModel
{
    public class RepairDto
    {
        public string Id { get; set; }
        public string DeviceSn { get; set; }
        public string DeviceName { get; set; }
        /// <summary>
        /// 关联的问题单编号
        /// </summary>
        public int? IssueId { get; set; }
        public int? RepairType { get; set; }
        public int? RepairStatus { get; set; }
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
        public int? EmergenceStatus { get; set; }
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
}
