using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HXCloud.ViewModel
{
    public class OpsFaultTypeEditDto
    {
        [Required]
        public int Id { get; set; }//用于表示是否是父节点
        [Required]
        public string FaultTypeName { get; set; }//运维故障类型名称
    }
}
