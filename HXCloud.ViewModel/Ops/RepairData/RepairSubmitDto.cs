using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.ViewModel
{
    public class RepairSubmitDto: AddRepairDataMessageDto
    {
        public string Url { get; set; }//关联文件的地址
        public string FaultCode { get; set; }
    }
}
