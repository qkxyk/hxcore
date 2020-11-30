using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.ViewModel
{
    public class DeviceStatisticsDto
    {
        public string Id { get; set; }
        public string DeviceSn { get; set; }
        public DateTime Date { get; set; }//时间初步设定每小时一次
        public string Data { get; set; }//要统计的所有数据，根据历史数据和类型要统计的数据生成一个json格式
        public int? ProjectId { get; set; } = 0;
    }
}
