using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HXCloud.ViewModel
{
    public class OpsFaultEditDto
    {
        [Required]
        public string Code { get; set; }//故障代码
        public string Description { get; set; }//故障描述
        public string Solution { get; set; }//故障解决方法
        [Required]
        public int OpsFaultTypeId { get; set; }//故障类型标识
    }
}
