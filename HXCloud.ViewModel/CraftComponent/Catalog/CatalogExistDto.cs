using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.ViewModel
{
    /// <summary>
    /// 工艺组件类型检测
    /// </summary>
    public class CatalogExistDto
    {
        public bool IsLeaf { get; set; }//是否叶子节点
        public bool IsExist { get; set; }//是否存在
        public bool HasExistElement { get; set; }//工艺组件类型是否存在组件
        public bool HasChild { get; set; }//是否有子节点
        public int CraftType { get; set; }
    }
}
