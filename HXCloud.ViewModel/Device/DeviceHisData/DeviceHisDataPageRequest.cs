using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.ViewModel
{
    public class DeviceHisDataPageRequest:BasePageRequest
    {
        //日期默认为当前日期
        public DateTime Begin { get; set; } = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 00:00:00"));
        public DateTime End { get; set; } = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 23:59:59"));
    }
}
