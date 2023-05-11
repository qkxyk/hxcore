using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HXCloud.ViewModel
{
    public class IssueUpdateRequest
    {
        [Required(ErrorMessage ="问题单编号不能为空")]
        public int Id { get; set; }
        public bool Status { get; set; } = true;
        public string Opinion { get; set; }
    }
}
