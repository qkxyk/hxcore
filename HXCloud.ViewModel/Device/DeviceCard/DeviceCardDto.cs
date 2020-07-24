using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.ViewModel
{
    public class DeviceCardDto
    {
        public string CardNo { get; set; }//卡号
        public string AppId { get; set; }
        public string AppSecret { get; set; }
        public DateTime? ExpireTime { get; set; }
        public string DeviceSn { get; set; }
    }
}
