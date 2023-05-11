using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.ViewModel
{
    public class RepairPartDto
    {
        public int Id { get; set; }
        public string RepairId { get; set; }
        public int Num { get; set; }//配件数量
        public string Data { get; set; }//配件数据，因为无法确定配件规格，暂用Data记录运维的配件数据
        public string Operate { get; set; }
        public DateTime OperateTime { get; set; }
    }
}
