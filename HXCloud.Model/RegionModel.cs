using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.Model
{
    //新系统剔除地域，只保留区域
    public class RegionModel : BaseModel, IAggregateRoot
    {
        public string Id { get; set; }//区域标示
        public string Name { get; set; }//区域名称
        public string Point { get; set; }//区域中心点
        public string Radius { get; set; }//区域半径
        public string ParentId { get; set; }//父区域标示
        public string FullPath { get; set; }//区域的完整路径
        public GroupModel Group { get; set; }
        public string GroupId { get; set; }
        public string DeleteId { get; set; }//记录已删除的区域标示
        public virtual ICollection<RegionModel> Child { get; set; }
        public virtual RegionModel Parent { get; set; }
    }
}
