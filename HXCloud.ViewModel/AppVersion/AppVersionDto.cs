using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.ViewModel
{
    public class AppVersionDto
    {
        public int Id { get; set; }
        public string VersionNo { get; set; }
        public string Descrption { get; set; }
        public bool State { get; set; } = false;//是否强制升级
        public string Address { get; set; }
        public int Type { get; set; } = 0;//升级文件类型
    }
}
