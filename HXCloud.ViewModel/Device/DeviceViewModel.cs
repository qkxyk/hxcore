using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.ViewModel
{
    //单个设备的返回，包含设备信息和设备的附属信息
    public class DeviceViewModel : DeviceDataDto
    {
        public List<TypeImageData> TypeImage { get; set; }
    }
}
