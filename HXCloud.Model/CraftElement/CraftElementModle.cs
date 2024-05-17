using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.Model
{

    /// <summary>
    /// 工艺组件，工艺组件模块化
    /// </summary>
    public class CraftElementModle : BaseModel, IAggregateRoot
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }//图片格式，有可能为空(svg格式，svg内容使用者会放在data中)
        public string Data { get; set; }//组件内容，使用者自己定义内容
        public int? UserId { get; set; }//个人组件用户标识

        public int CatalogId { get; set; }
        public CraftType ElementType { get; set; } = 0;
        public virtual CraftComponentCatalogModle CraftComponentCatalog { get; set; }//关联的组件类型
    }
    /// <summary>
    /// 组件类型,工艺分类上指明该类型不能添加和工艺组件不同的工艺
    /// </summary>
    public enum CraftType
    {
        Open, Personal
    }


    /// <summary>
    /// 工艺组件分类,初步分为基础组件、合成组件，从另一个维度可以分为个人组件和公共组件，其中基础组件和组合组件属于公共组件，
    /// 因此工艺组件类型只设基础组件、合成组件和个人组件。
    /// </summary>
    public class CraftComponentCatalogModle : BaseModel, IAggregateRoot
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }//组件类型图标
        public int? ParentId { get; set; }
        public virtual CraftComponentCatalogModle Parent { get; set; }
        public virtual ICollection<CraftComponentCatalogModle> Child { get; set; }
        public virtual ICollection<CraftElementModle> CraftElements { get; set; }
        public CraftCatalogType CraftCatalogType { get; set; } = 0;
        public CraftType ElementType { get; set; } = 0;//指明该工艺类型是否可以添加不同的工艺或者工艺类型
    }

    /// <summary>
    /// 工艺类型种类，叶子和根节点，叶子节点和根节点都可以挂载工艺组件，
    /// 区别在于根节点可以添加叶子节点，叶子节点不可以再添加节点
    /// </summary>
    public enum CraftCatalogType
    {
        Root, Leaf
    }
}
