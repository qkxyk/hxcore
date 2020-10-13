using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.ViewModel
{
    public class WarnCodeDto
    {
        public string Description { get; set; }//报警描述
        public string Code { get; set; }//报警编码,报警、故障或者通知编码，唯一
        public int WarnTypeId { get; set; }
        public string WarnTypeName { get; set; }
    }
}
