using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.ViewModel
{
    /// <summary>
    /// 运维单流程数据包含备注信息
    /// </summary>
    public class AddRepairDataMessageDto : AddRepairDataBaseDto
    {
        public string Message { get; set; }//维修信息
    }
}
