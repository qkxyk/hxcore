using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.ViewModel
{
    public class DebugStatisticsDto
    {
        /// <summary>
        /// 总数
        /// </summary>
        public int Total { get; set; }
        /// <summary>
        /// 已完成
        /// </summary>
        public int Complete { get; set; }
        /// <summary>
        /// 未完成
        /// </summary>
        public int InComplete { get; set; }
    }
}
