using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.ViewModel
{
    public class RepairAndIssueDto : RepairDto
    {
        /// <summary>
        /// 问题描述
        /// </summary>
        public string IssueDescription { get; set; }
        /// <summary>
        /// 问题图片，支持多张图片
        /// </summary>
        public string IssueUrl { get; set; }
        public DateTime IssueDt { get; set; }
        /// <summary>
        /// 提交人
        /// </summary>
        public string Submitter { get; set; }
    }
}
