using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.ViewModel
{
    public class IssueDto
    {
        public int Id { get; set; }
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
        public DateTime Dt { get; set; }
        /// <summary>
        /// 显示问题状态是否已经处理
        /// </summary>
        public bool Status { get; set; }
        /// <summary>
        /// 处理意见
        /// </summary>
        public string Opinion { get; set; }
        /// <summary>
        /// 问题单提交人
        /// </summary>
        public string Create { get; set; }
        /// <summary>
        /// 问题单处理人
        /// </summary>
        public string Handle { get; set; }
        public string HandleName { get; set; }//问题单处理人姓名
        public string ProjectName { get; set; }
        public string CreateName { get; set; }
    }
}
