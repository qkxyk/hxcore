using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HXCloud.ViewModel
{
    public class DeviceMonitorDataRequestDto
    {
        public DateTime? Dt { get; set; }
        //日期默认为当天

        public DateTime Begin { get; set; } = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 00:00:00"));
        public DateTime End { get; set; } = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 23:59:59"));
        //public string DeviceSn { get; set; }//设备序列号
        public DeviceMonitorDataRequestDto()
        {
        }

        public void GetDate()
        {
            if (Dt.HasValue)
            {
                Begin = Convert.ToDateTime(Dt.Value.ToString("yyyy-MM-dd 00:00:00"));
                End = Convert.ToDateTime(Dt.Value.ToString("yyyy-MM-dd 23:59:59"));
            }
        }
    }
}
