using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.Model
{
    public class DeviceHisDataModel : BaseModel, IAggregateRoot
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
}
