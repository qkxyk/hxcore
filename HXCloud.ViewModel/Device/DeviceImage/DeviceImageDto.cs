using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.ViewModel
{
    public class DeviceImageDto
    {
        public int Id { get; set; }
        public string ImageName { get; set; }
        public string url { get; set; }
        public int Rank { get; set; } = 1;//图片顺序

        public string DeviceSn { get; set; }//设备序列号
    }
}
