using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.Model
{
    public class RoleProjectModel : BaseCModel/* IAggregateRoot*/
    {
        public int RoleId { get; set; }//角色编号
        public int ProjectId { get; set; }//项目或者场站编号
        public ProjectOperate Operate { get; set; }//操作
        public virtual RoleModel Role { get; set; }//角色信息
        public virtual ProjectModel Project { get; set; }//项目信息
    }
    //项目操作权限
    public enum ProjectOperate
    {
        View, Control, Modify, Add, Delete
    }
}
