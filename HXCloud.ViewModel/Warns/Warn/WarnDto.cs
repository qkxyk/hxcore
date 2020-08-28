using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.ViewModel
{
    public class WarnDto
    {
        public string Modify { get; set; }//报警处理人
        public DateTime? ModifyTime { get; set; }     //报警处理时间
        public int Id { get; set; }
        public string Code { get; set; }//外键，报警编码 
        public string CodeDescription { get; set; }
        public int WarnTypeId { get; set; }//报警类型
        public string WarnTypeName { get; set; }
        public DateTime Dt { get; set; } = DateTime.Now;//报警时间
        public string DeviceSn { get; set; }//设备编码
        public string DeviceNo { get; set; }//设备序列号
        public bool State { get; set; } = false;//报警状态
        public string Comments { get; set; }//报警处理意见
    }
}
