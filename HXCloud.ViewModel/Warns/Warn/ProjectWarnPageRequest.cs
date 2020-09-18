using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.ViewModel
{
    public class ProjectWarnPageRequest : BasePageRequest
    {
        public DateTime BeginTime { get; set; } = DateTime.Now.AddYears(-1);
        public DateTime EndTime { get; set; } = DateTime.Now;
        public int? ProjectId { get; set; }//如果没有输入则为我的设备
        public int State { get; set; } = 0;//报警是否处理过，0表示未处理，1表示已处理
    }
}
