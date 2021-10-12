using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.Model
{
    //数采仪上传数据（只做设备小时数据）
   public class DeviceMonitorDataModel:IAggregateRoot
    {
        public Guid Id { get; set; }//序号，无意义
     
        public WaterType WaterType { get; set; }//进水或出水
        public string Content { get; set; }//数据整体保存（json格式）
        public string TimeSn { get; set; }//时间序号，用以区分重复数据
        public DateTime Date { get; set; } = DateTime.Now;//数据上传时间
        public string DeviceSn { get; set; }//设备序列号
        public virtual DeviceModel Device { get; set; }//关联的设备数据
        public string Mn { get; set; }//数采仪编号

    }
    //进水或者出水
    public enum WaterType
    {
        In=1,
        Out
    }
    /// <summary>
    /// 数采仪按天的数据
    /// </summary>
    public class DeviceDayMonitorDataModel : IAggregateRoot
    {
        public Guid Id { get; set; }//序号，无意义

        public WaterType WaterType { get; set; }//进水或出水
        public string Content { get; set; }//数据整体保存（json格式）
        public string TimeSn { get; set; }//时间序号，用以区分重复数据
        public DateTime Date { get; set; } = DateTime.Now;//数据上传时间
        public string DeviceSn { get; set; }//设备序列号
        public virtual DeviceModel Device { get; set; }//关联的设备数据
        public string Mn { get; set; }//数采仪编号
    }
}
