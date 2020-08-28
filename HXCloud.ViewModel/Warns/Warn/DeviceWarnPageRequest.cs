using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.ViewModel
{
    public class DeviceWarnPageRequest : BasePageRequest
    {
        public DateTime BeginTime { get; set; }
        public DateTime EndTime { get; set; }
        //public string DeviceSn { get; set; }
        //public string Code { get; set; }

        public int State { get; set; } = 0;//报警是否处理过，0表示未处理，1表示已处理
    }
}
