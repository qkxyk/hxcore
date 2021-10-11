using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.ViewModel
{
    public class DeviceHisDataDto
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
