using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.ViewModel
{
    public class DeviceLogPageRequest : BasePageRequest
    {
        public DateTime? Begin { get; set; }
        public DateTime? End { get; set; }
    }
}
