using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.ViewModel
{
    /// <summary>
    /// 故障数据，单独使用
    /// </summary>
    public class OpsFaultDto:OpsFaultData
    {
        public int OpsFaultTypeId { get; set; }//故障类型标识
        public string OpsFaultTypeName { get; set; }//故障类型名称
    }
    /// <summary>
    /// 故障数据，主要用来作为故障类型中关联的故障数据
    /// </summary>
    public class OpsFaultData
    {
        public string Code { get; set; }//故障代码
        public string Description { get; set; }//故障描述
        public string Solution { get; set; }//故障解决方法
    }
}
