using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HXCloud.ViewModel
{
    public class DepartmentUpdateViewModel
    {
        [Required(ErrorMessage = "{0}不能为空")]
        [Display(Name = "部门名称")]
        public string Name { get; set; }//部门名称
        [Required(ErrorMessage = "{0}不能为空")]
        [Display(Name = "部门标识")]
        public int Id { get; set; }
        public string Description { get; set; }

    }
}
