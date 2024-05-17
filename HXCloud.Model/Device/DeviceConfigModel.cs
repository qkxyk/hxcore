using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.Model
{
    //设备配置数据，原设备baseData，设备的配置数据要类型的配置数据综合在一起
    public class DeviceConfigModel : BaseModel, IAggregateRoot
    {
        public int Id { get; set; }
        public string DataName { get; set; }
        public string DataType { get; set; }//配置类型，使用者定义
        public string DataValue { get; set; }
        public string DeviceSn { get; set; }
        public virtual DeviceModel Device { get; set; }
        public int Category { get; set; } = 1;//设备默认为1，类型的数据默认为0
        public string Position { get; set; }//配置数据的坐标
    }
}
