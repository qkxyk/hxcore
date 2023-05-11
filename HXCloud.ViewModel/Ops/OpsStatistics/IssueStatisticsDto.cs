using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.ViewModel
{
    /// <summary>
    /// 问题单处理情况统计
    /// </summary>
    public class IssueStatisticsDto
    {
        /// <summary>
        /// 已处理
        /// </summary>
        public int Handled { get; set; }
        /// <summary>
        /// 未处理
        /// </summary>
        public int UnHandle { get; set; }
    }
}
