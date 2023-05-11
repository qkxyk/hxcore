using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.ViewModel
{
    /// <summary>
    ///维修工单流程数据
    /// </summary>
    public class RepairDataDto : BaseRepairDataDto
    {
        public string Message { get; set; }//维修信息
        public string Url { get; set; }//关联文件的地址
  

    }
}
