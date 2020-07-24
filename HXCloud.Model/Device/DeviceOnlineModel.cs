using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.Model
{
    //设备在线功能通过设备上线和设备下线时发送的遗言来维护
    public class DeviceOnlineModel : IAggregateRoot
    {
        public string DeviceSn { get; set; }
        public string DeviceNo { get; set; }
        public DateTime dt { get; set; } = DateTime.Now;//设备上次在线时间
        public DateTime? OffLine { get; set; }//设备上次离线时间
        public string DataContent { get; set; }
        public string DataTitle { get; set; }
        public bool State { get; set; }//设备在线状态

        public string GroupId { get; set; }
        public virtual GroupModel Group { get; set; }

        public virtual DeviceModel Device { get; set; }
    }
}
