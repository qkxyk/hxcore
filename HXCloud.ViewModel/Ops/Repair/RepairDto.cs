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
        public string ProjectName { get; set; }
        /// <summary>
        /// 关联的问题单编号
        /// </summary>
        public int? IssueId { get; set; }
        //public string IssueDescription { get; set; }
        //public DateTime IssueDate { get; set; }
        public string RepairType { get; set; }
        public string RepairStatus { get; set; }
        ///// <summary>
        ///// 接单人
        ///// </summary>      
        public string Receiver { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 完成时间
        /// </summary>
        public DateTime? CompleteTime { get; set; }

        /// <summary>
        /// 接单人电话
        /// </summary>
        public string ReceivePhone { get; set; }
        public string EmergenceStatus { get; set; }
        public string Description { get; set; }//提交描述
        public string CreateName { get; set; }//冗余创建者
        public string ReceiverName { get; set; }//冗余接单人
        public List<RepairDataDto> RepairData { get; set; }
        /// <summary>
        /// 关联问题单数据
        /// </summary>
        //public IssueData IssueData { get; set; }
        public string FaultCode { get; set; }//故障代码，此功能主要用来统计故障数据和故障代码弱关联
    }
}
