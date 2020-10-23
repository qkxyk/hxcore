using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.ViewModel
{
    public class DeviceLogAddDto
    {
        public DateTime SendTime { get; set; }//发送时间
        public DateTime Time { get; set; }                       //指令操作返回时间

        public string Key { get; set; }//操作的key
        public string KeyName { get; set; }
        public string Value { get; set; }//发送值
        public string OldValue { get; set; }       //原来值
        public string NewValue { get; set; }                  //返回值
        public string DeviceSn { get; set; }
    }
}
