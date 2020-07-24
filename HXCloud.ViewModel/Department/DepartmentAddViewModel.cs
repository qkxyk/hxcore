using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HXCloud.ViewModel
{
    public class DepartmentAddViewModel
    {
        [Required(ErrorMessage = "{0}不能为空")]
        [Display(Name = "部门名称")]
        public string Name { get; set; }//部门名称
        public int? ParentId { get; set; }//父部门标示
        public string Description { get; set; }
        [Required(ErrorMessage = "部门所属的组织编号不能为空")]
        public string GroupId { get; set; }//组织标示,如果不输入表示添加本组织的部门

        [Range(0, 1, ErrorMessage = "类型只能为0和1")]
        public int DepartmentType { get; set; } = 0;//默认为部门
    }
}
