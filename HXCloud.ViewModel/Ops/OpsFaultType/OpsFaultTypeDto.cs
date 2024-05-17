using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.ViewModel
{
    /// <summary>
    /// 故障类型叶子节点
    /// </summary>
    public class OpsFaultTypeDto
    {
        public int FaultTypeId { get; set; }//运维故障类型编号
        //public int Flag { get; set; } = 1;//用于表示是否是父节点
        public string FaultTypeName { get; set; }//运维故障类型名称
        public int? ParentId { get; set; }//父节点编号
        public List<OpsFaultDto> OpsFaults { get; set; }//包含的故障数据
    }
    /// <summary>
    /// 故障类型顶级节点
    /// </summary>
    public class OpsFaultTypeParentDto
    {
        public int FaultTypeId { get; set; }//运维故障类型编号
        public string FaultTypeName { get; set; }//运维故障类型名称
        public List<OpsFaultTypeDto> Child { get; set; }
    }

    public class OpsFaultTypeChildDto
    {
        public int FaultTypeId { get; set; }//运维故障类型编号
        public string FaultTypeName { get; set; }//运维故障类型名称
    }
}
