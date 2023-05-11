using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HXCloud.ViewModel
{
    public class RepairRequest:BaseRequest
    {
        [Range(0,1,ErrorMessage ="运维类型只能是{1}和{2}")]
        public int RepairType { get; set; } = 0;//类型，0为维修，1为调试
        [Range(0,5,ErrorMessage ="运维状态只能在{1}和{2}之间")]
        public int RepairStatus { get; set; } = 0;
        public string Account { get; set; }
    }
}
