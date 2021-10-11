using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.ViewModel
{
    public class DeviceHisDataPageRequest
    {
        //日期默认为当前日期
        public DateTime Date { get; set; } = DateTime.Now;
    }
}
