using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.Model
{
    public class MenuModel : BaseModel, IAggregateRoot
    {

        public int Id { get; set; }//菜单标示
        public string Name { get; set; }//菜单名称
        public int? ParentId { get; set; }//父菜单标示
        public virtual MenuModel Parent { get; set; }//父菜单
        public string Icon { get; set; }//菜单图标
        public virtual ICollection<MenuModel> Child { get; set; }//子菜单集合

        public string GroupId { get; set; }//所属组织标示
        public virtual GroupModel Group { get; set; }//所属组织
        public virtual ICollection<RoleMenuModel> RoleMenus { get; set; }
    }
}
