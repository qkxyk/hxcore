using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.Model
{
    public class DeviceHisDataModel : IAggregateRoot
    {
        public int Id { get; set; }
        public string DeviceSn { get; set; }
        // public string DeviceNo { get; set; }
        public DateTime Dt { get; set; } = DateTime.Now;//设备上次在线时间
        public string DataContent { get; set; }
        public string DataTitle { get; set; }

        public string GroupId { get; set; }
        public virtual GroupModel Group { get; set; }

        public virtual DeviceModel Device { get; set; }
    }

    /// <summary>
    /// mysql数据结构
    /// </summary>
    public class CoreDeviceHisDataMoel
    {
        public string DeviceSn { get; set; }
        public string GroupId { get; set; }

        public int Id { get; set; }
        public DateTime Dt { get; set; } = DateTime.Now;
        public string DataContent { get; set; }//数据包内容
                                               //public DataContent DataContent { get; set; }
        public string DataTitle { get; set; }//数据包主题      
    }
}
