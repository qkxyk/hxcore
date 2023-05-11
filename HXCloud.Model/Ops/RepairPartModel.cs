using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.Model
{
    /// <summary>
    /// 运维配件表
    /// 运维部没有给出具体配件数据，故先设置运维配件手动输入，后期有具体数据之后再做更改
    /// </summary>
    public class RepairPartModel:IAggregateRoot
    {
        public int Id { get; set; }
        public string RepairId { get; set; }
        public int Num { get; set; }//配件数量
        public string Data { get; set; }//配件数据，因为无法确定配件规格，暂用Data记录运维的配件数据
        public string Operate { get; set; }
        public DateTime OperateTime { get; set; }
        public virtual RepairModel Repair { get; set; }//关联运维单
    }
}
