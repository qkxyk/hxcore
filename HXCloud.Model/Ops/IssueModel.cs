using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.Model
{
    public class IssueModel :BaseModel, IAggregateRoot
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
        //public string Account { get; set; }
        public DateTime Dt { get; set; }
        /// <summary>
        /// 显示问题状态是否已经处理
        /// </summary>
        public bool Status { get; set; }
        /// <summary>
        /// 处理意见
        /// </summary>
        public string Opinion { get; set; }

    }
}
