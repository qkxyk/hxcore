using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.Model
{
    public class RoleMenuModel : BaseCModel /*IAggregateRoot*/
    {
        public int RoleId { get; set; }//角色标示
        public int MenuId { get; set; }//菜单标示
        public MenuOperate Operate { get; set; }//菜单操作权限
        public virtual RoleModel Role { get; set; }//角色信息
        public virtual MenuModel Menu { get; set; }//菜单信息
    }
    public enum MenuOperate
    {
        View = 0, Add, Modify, Delete
    }
}
