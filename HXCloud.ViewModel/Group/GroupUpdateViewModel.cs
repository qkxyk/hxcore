using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HXCloud.ViewModel
{
    public class GroupUpdateViewModel
    {
        //不允许修改组织代码
        [Required(ErrorMessage ="组织标示不能为空")]
        public string GroupId { get; set; }//组织代码不为空时超级管理员可以修改
        [Required(ErrorMessage = "{0}不能为空")]
        [StringLength(100, ErrorMessage = "{0}长度在2和100之间", MinimumLength = 2)]
        [Display(Name = "组织名称")]
        public string GroupName { get; set; }
        public string Description { get; set; }
    }
}
