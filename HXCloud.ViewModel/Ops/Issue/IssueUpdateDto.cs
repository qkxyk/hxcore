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
        [Required]
        public int Id { get; set; }
        public bool Status { get; set; } = true;
        public string Opinion { get; set; }
    }
}
