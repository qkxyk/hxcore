using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HXCloud.ViewModel
{
    public class RepairStatusUpdateDto
    {
        [Required(ErrorMessage = "运维状态必须输入")]
        [Range(0, 4, ErrorMessage = "{0}只能输入{1}到{2}")]
        public int RepairStatus { get; set; }
        [Required(ErrorMessage = "运维单编号不能为空")]
        public string Id { get; set; }
    }
}
