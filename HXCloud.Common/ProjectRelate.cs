using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.Common
{
    /// <summary>
    /// 项目规模
    /// </summary>
    public class ProjectScale
    {
        public string Unit { get; set; }//单位
        public decimal Num { get; set; }
        public override string ToString()
        {
            return $"{Num}{Unit}";
        }
    }
}
