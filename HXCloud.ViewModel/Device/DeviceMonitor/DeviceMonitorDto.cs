using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.ViewModel
{
    //设备集采仪数据
    public class DeviceMonitorDto
    {
        public Guid Id { get; set; }//序号，无意义

        public string WaterType { get; set; }//进水或出水
        public string Content { get; set; }//数据整体保存（json格式）
        public string TimeSn { get; set; }//时间序号，用以区分重复数据
        public DateTime Date { get; set; } = DateTime.Now;//数据上传时间
        public string DeviceSn { get; set; }//设备序列号
    }
}
