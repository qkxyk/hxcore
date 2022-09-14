using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.Model
{
    //设备操作日志，通过web端或者移动端发送设备操作日志
    public class DeviceLogModel : BaseModel, IAggregateRoot
    {
        public int Id { get; set; }
        public string Account { get; set; }
        public DateTime SendTime { get; set; }//发送时间
        public DateTime Time { get; set; }                       //指令操作返回时间

        public string Key { get; set; }//操作的key
        public string KeyName { get; set; }
        public string Value { get; set; }//发送值
        public string OldValue { get; set; }       //原来值
        public string NewValue { get; set; }                  //返回值
        public string DeviceSn { get; set; }
        public virtual DeviceModel Device { get; set; }
    }

    //用于转换组合数据
    public class DeviceLogData
    {
        public int Id { get; set; }
        public string Account { get; set; }
        public DateTime SendTime { get; set; }//发送时间
        public DateTime Time { get; set; }                       //指令操作返回时间

        public string Key { get; set; }//操作的key
        public string KeyName { get; set; }
        public string Value { get; set; }//发送值
        public string OldValue { get; set; }       //原来值
        public string NewValue { get; set; }                  //返回值
        public string DeviceSn { get; set; }
        public string UserName { get; set; }
        
    }
}
