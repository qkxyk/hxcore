using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HXCloud.ViewModel
{
    public class OpsFaultTypeAddDto
    {
        [Required]
        public int Flag { get; set; } = 1;//用于表示是否是父节点
        [Required]
        public string FaultTypeName { get; set; }//运维故障类型名称
        public int? ParentId { get; set; }//父节点编号
    }
}
