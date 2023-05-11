using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HXCloud.ViewModel
{
    /// <summary>
    /// 更改运维单状态
    /// </summary>
    public class IssueUpdateDto
    {
        public int Id { get; set; }
        public bool Status { get; set; } = true;
        public string Opinion { get; set; }
        public string HandleName { get; set; }//问题单处理人姓名
    }
}
