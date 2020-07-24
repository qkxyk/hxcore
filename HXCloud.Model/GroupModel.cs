using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.Model
{
    public class GroupModel : BaseModel, IAggregateRoot
    {
        public string Id { get; set; }//组织编号
        public string GroupName { get; set; }//组织名称
        public string GroupCode { get; set; }//组织代码
        public string Logo { get; set; }//组织logo
        public string Description { get; set; }//组织备注
        public virtual ICollection<UserModel> Users { get; set; }
        public virtual ICollection<RoleModel> Roles { get; set; }
        public virtual ICollection<TypeModel> Types { get; set; }
        public virtual ICollection<DepartmentModel> Departments { get; set; }
        public virtual ICollection<ProjectModel> Projects { get; set; }
        public virtual ICollection<DeviceModel> Devices { get; set; }
        public virtual ICollection<RegionModel> Regions { get; set; }
        /* 

           public virtual ICollection<MenuModel> Menus { get; set; }
       
           
           */
    }
}
