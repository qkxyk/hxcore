using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HXCloud.ViewModel
{
    public class PatrolItemRequest
    {
        /// <summary>
        /// 指示该数据属于那个模块
        /// </summary>
        [Range(0, 3, ErrorMessage = "巡检类型必须在 {1} 和 {2}之间.")]
        public int OpsType { get; set; }
        [Required(ErrorMessage = "设备编号不能为空")]
        public string DeviceSn { get; set; }
    }
}
