using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.ViewModel
{
    /// <summary>
    /// 转换从添加数据到model
    /// </summary>
    public class IssueAddDto
    {
        public string DeviceSn { get; set; }
        public string DeviceName { get; set; }
        /// <summary>
        /// 问题描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 问题图片，支持多张图片
        /// </summary>
        public string Url { get; set; }
    }
}
