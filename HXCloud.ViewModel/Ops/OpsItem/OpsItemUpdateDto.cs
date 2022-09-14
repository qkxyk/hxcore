using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HXCloud.ViewModel
{
    public class OpsItemUpdateDto
    {
        [Required(ErrorMessage ="巡检项目标识不能为空")]
        public int Id { get; set; }
        [Required(ErrorMessage = "巡检项目名称不能为空")]
        public string Name { get; set; }
        /// <summary>
        /// 数据类型
        /// </summary>
        public string DataType { get; set; }
        /// <summary>
        /// 指示该数据属于那个模块
        /// </summary>
        public int OpsType { get; set; }
        public string Unit { get; set; }
        public float Max { get; set; }
        public float Min { get; set; }
    }
}
