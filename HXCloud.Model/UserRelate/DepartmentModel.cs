using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.Model
{
    public class DepartmentModel : BaseModel, IAggregateRoot
    {
        public int Id { get; set; }//部门标示
        public string DepartmentName { get; set; }//部门名称
        public int? ParentId { get; set; }//父部门标示
        public virtual DepartmentModel Parent { get; set; }//父部门

        public virtual ICollection<DepartmentModel> Child { get; set; }//子部门集合
        #region 2019-12-24新增部门层级
        public int Level { get; set; } //默认层级为0
        public string PathId { get; set; }//父层级id，中间以/分割
        public string PathName { get; set; }//父层级路径，中间以/分割
        public DepartmentType DepartmentType { get; set; }
        #endregion
        public string Description { get; set; }
        public string GroupId { get; set; }//组织标示
        public virtual GroupModel Group { get; set; }//部门所属组织
        public virtual ICollection<UserDepartmentModel> UserDepartments { get; set; }//用户和组织是多对多关系，一个用户可以分属多个部门


    }
    //0为正常部门，1为部门岗位,岗位下不能再有子节点
    public enum DepartmentType
    {
        Normal, Station
    }
}
