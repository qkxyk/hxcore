using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.ViewModel
{
    public class DeviceCardDto
    {
        public int Id { get; set; }
        public string CardNo { get; set; }//卡号
        public string AppId { get; set; }
        public string AppSecret { get; set; }
        public DateTime? ExpireTime { get; set; }
        public string DeviceSn { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string ICCID { get; set; }
        public string IMEI { get; set; }
    }
}
