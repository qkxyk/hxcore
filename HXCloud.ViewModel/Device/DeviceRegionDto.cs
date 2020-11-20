using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.ViewModel
{
    /// <summary>
    /// 区域设备信息
    /// </summary>
    public class DeviceRegionDto
    {

        public List<DeviceTypeInfo> TypeData { get; set; }
        public List<RegionTypeInfo> RegionData { get; set; }
    }
}
