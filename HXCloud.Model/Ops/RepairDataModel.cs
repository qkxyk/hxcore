using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.Model
{
    /// <summary>
    /// 记录工单流程数据
    /// </summary>
    public class RepairDataModel:IAggregateRoot
    {
        public string Id { get; set; }
        public string RepairId { get; set; }//关联工单编号
        public string Message { get; set; }//维修信息
        public string Url { get; set; }//关联文件的地址
        public int Sn { get; set; } = 1;//维修序号,默认为1
        public string Operator { get; set; }//操作人
        public DateTime OperDate { get; set; }//操作时间
        public RepairStatus RepairStatus { get; set; }

        public virtual RepairModel Repair { get; set; }//关联的维修单
    }
}
