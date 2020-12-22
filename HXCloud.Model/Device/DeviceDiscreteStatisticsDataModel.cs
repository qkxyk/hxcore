using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.Model
{
    //离散型统计数据，目的把离散型数据从原统计数据中剔除出来
    public class DeviceDiscreteStatisticsDataModel:IAggregateRoot
    {
        public string Id { get; set; }
        public string DeviceSn { get; set; }
        public DateTime Date { get; set; }//时间初步设定每小时一次
        public string Data { get; set; }//要统计的所有数据，根据历史数据和类型要统计的数据生成一个json格式
        public virtual DeviceModel Device { get; set; }
        public int? ProjectId { get; set; } = 0;
        public DeviceDiscreteStatisticsDataModel()
        {
            Id = Guid.NewGuid().ToString("N");
        }
    }
}
