using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.ViewModel
{
    /// <summary>
    /// 设备总揽数据
    /// </summary>
    public class DeviceOverViewDto
    {
        public DeviceOverViewDto()
        {
            TypeData = new List<DeviceTypeInfo>();
            //Data = new List<DeviceOverView>();
        }
        //public List<DeviceOverView> Data { get; set; }
        public int Num { get; set; }//设备总数量
        public int OnlineNum { get; set; }//在线设备总数量
        public List<DeviceTypeInfo> TypeData { get; set; }
    }
}
