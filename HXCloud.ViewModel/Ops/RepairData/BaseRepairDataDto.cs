using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.ViewModel
{
    /// <summary>
    /// 维修工单基本的流程数据
    /// </summary>
    public class BaseRepairDataDto
    {
        public string Operator { get; set; }//操作人
        public DateTime OperDate { get; set; }//操作时间
        public string repairStatus { get; set; }
        public string RepairId { get; set; }//关联工单编号
        public int Sn { get; set; } = 1;//维修序号,默认为1
    }
}
