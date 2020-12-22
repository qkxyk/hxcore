using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HXCloud.ViewModel
{
    public class TypeStatisticsAddViewModel
    {
        [Required(ErrorMessage = "类型数据定义标示不能为空")]
        public int DataDefineId { get; set; }
        //[Required(ErrorMessage = "Key不能为空")]
        //public string DataKey { get; set; }//要统计的数据,
        [Required(ErrorMessage = "统计的类型不能为空")]
        [Range(1, 3, ErrorMessage = "要统计的类型只能为1、2和3")]
        public int StaticsType { get; set; }//要统计的是瞬时值或是累计值,1为瞬时值，2为累计值
        public string Standard { get; set; }//标准值
        //public string SUnit { get; set; }//标准值单位
        [Required(ErrorMessage = "统计名称不能为空")]
        public string Name { get; set; }//要统计的数据的名称
        [Range(0, 2, ErrorMessage = "0为检测值，1为监测，2为状态")]
        public int DisplayType { get; set; } = 0;//初步定义0为检测值，1为监测，2为状态
        [Range(0, 3, ErrorMessage = "0表示设备级，1代表场站级，2代表项目级，3代表首页级，他们是包含级别，1包含0和1")]
        public int ShowState { get; set; } //0表示设备级，1代表场站级，2代表项目级，3代表首页级，他们是包含级别，1包含0和1
        public string Filter { get; set; }//过滤的内容
        [Range(0, 2, ErrorMessage = "0为不过滤，1为最大值最小值过滤，2为其他方案")]
        public int FilterType { get; set; } = 0; //过滤方案，0为不过滤，1为最大值最小值过滤，2为其他方案
        public string Description { get; set; }
    }
}
