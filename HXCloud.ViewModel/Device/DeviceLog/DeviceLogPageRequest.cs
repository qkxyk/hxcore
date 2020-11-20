using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.ViewModel
{
    public class DeviceLogPageRequest : BasePageRequest
    {
        public DateTime BeginTime { get; set; } = DateTime.Now.AddMonths(-1);
        public DateTime EndTime { get; set; } = DateTime.Now;
    }
}
