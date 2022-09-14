using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HXCloud.ViewModel
{
    public class RepairPageRequest:BasePageRequest
    {
        [Range(0, 1, ErrorMessage = "运维类型只能是{0}和{1}")]
        public int RepairType { get; set; } = 0;//类型，0为维修，1为调试
        [Range(0, 4, ErrorMessage = "运维状态只能在{0}和{1}之间")]
        public int RepairStatus { get; set; } = 0;
        public string Account { get; set; }
    }
}
