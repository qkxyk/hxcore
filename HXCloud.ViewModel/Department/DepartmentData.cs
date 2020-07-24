using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.ViewModel
{
    public class DepartmentData
    {
        public DepartmentData()
        {
            Child = new List<DepartmentData>();
        }
        public int Id { get; set; }//部门标示
        public string Name { get; set; }//部门名称
        public int? ParentId { get; set; }//父部门标示
        public string GroupId { get; set; }//组织标示
        public int Level { get; set; } //默认层级为0
        public string PathId { get; set; }//父层级id，中间以/分割
        public string PathName { get; set; }//父层级路径，中间以/分割
        public string Description { get; set; }
        public List<DepartmentData> Child { get; set; }//部门子部门
    }
}
