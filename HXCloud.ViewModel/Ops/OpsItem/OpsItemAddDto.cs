using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HXCloud.ViewModel
{
    public class OpsItemAddDto
    {
        [Required(ErrorMessage = "巡检项目Key不能为空")]
        public string Key { get; set; }
        [Required(ErrorMessage = "巡检项目名称不能为空")]
        public string Name { get; set; }
        /// <summary>
        /// 数据类型
        /// </summary>
        public string DataType { get; set; }
        /// <summary>
        /// 指示该数据属于那个模块
        /// </summary>
        [Range(0,3,ErrorMessage = "巡检类型必须在 {1} 和 {2}之间.")]
        public int OpsType { get; set; }
        public string Unit { get; set; }
        public float Max { get; set; }
        public float Min { get; set; }
    }
}
