using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.Model
{
    //外部生成，不需要添加生成数据日期和修改日期
    public class DeviceStatisticsDataModel : IAggregateRoot
    {
        public string Id { get; set; }
        public string DeviceSn { get; set; }
        public DateTime Date { get; set; }//时间初步设定每小时一次
        public string Data { get; set; }//要统计的所有数据，根据历史数据和类型要统计的数据生成一个json格式
        public virtual DeviceModel Device { get; set; }
        public int? ProjectId { get; set; } = 0;
        public DeviceStatisticsDataModel()
        {
            Id = Guid.NewGuid().ToString("N");
        }
    }
}
