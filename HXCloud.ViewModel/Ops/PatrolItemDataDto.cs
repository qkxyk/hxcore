using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.ViewModel
{
    /// <summary>
    /// 对巡检数据进行序列化和反序列化
    /// </summary>
    public class  PatrolItemDataDto
    {
        public string Key { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        /// <summary>
        /// 数据单位
        /// </summary>
        public string Unit { get; set; }
        /// <summary>
        /// 生产数据、水质分析、设备巡检和工艺巡检
        /// </summary>
        public string OpsType { get; set; }
    }
}
