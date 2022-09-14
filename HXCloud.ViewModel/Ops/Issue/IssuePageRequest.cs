using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.ViewModel
{
    public class IssuePageRequest:BasePageRequest
    {
        /// <summary>
        /// 显示问题状态是否已经处理
        /// </summary>
        public bool Status { get; set; } = false;
    }
}
