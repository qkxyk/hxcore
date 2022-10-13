using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.ViewModel
{
    public class RepairPageRequestDto:BasePageRequest
    {
        public int RepairType { get; set; } = 0;//类型，0为维修，1为调试
        public int RepairStatus { get; set; } = 0;
        public List<string> Accounts { get; set; }
    }
}
