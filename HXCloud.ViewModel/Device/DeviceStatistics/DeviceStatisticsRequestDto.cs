using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.ViewModel
{
    public class DeviceStatisticsRequestDto
    {
        //日期默认为前一天
        public DateTime BeginTime { get; set; } = Convert.ToDateTime(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd 00:00:00"));
        public DateTime EndTime { get; set; }= Convert.ToDateTime(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd 23:59:59"));
        public bool IsDevice { get; set; } = false;//是否是统计设备
        public int ProjectId { get; set; } = 0;//默认为全部设备的统计
        public string DeviceSn { get; set; }//设备序列号
    }
    //设备离散统计，离散统计只做设备级（数据量比较大）
    public class DeviceDisStatisticsRequestDto
    {
        //日期默认为前一天
        public DateTime BeginTime { get; set; } = Convert.ToDateTime(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd 00:00:00"));
        public DateTime EndTime { get; set; } = Convert.ToDateTime(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd 23:59:59"));
        //public bool IsDevice { get; set; } = false;//是否是统计设备
        //public int ProjectId { get; set; } = 0;//默认为全部设备的统计
        public string DeviceSn { get; set; }//设备序列号
    }
}


