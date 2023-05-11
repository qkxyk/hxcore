using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.ViewModel
{
    /// <summary>
    /// 获取用户运维统计数据
    /// </summary>
    public class OpsStatisticsRequest
    {
        //public string? UserName { get; set; }
        //日期默认为前一天
        //public DateTime BeginTime { get; set; } = Convert.ToDateTime(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd 00:00:00"));
        //public DateTime EndTime { get; set; } = Convert.ToDateTime(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd 23:59:59"));
        //public bool IsMonth { get; set; } = true;
        //public OpsStatisticsType StatisticsType { get; set; } = OpsStatisticsType.Month;
    }

    public enum OpsStatisticsType
    {
        Week, Month, Year
    }
}
