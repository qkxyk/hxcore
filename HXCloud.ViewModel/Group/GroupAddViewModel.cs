using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HXCloud.ViewModel
{
    public class GroupAddViewModel
    {
        [Required(ErrorMessage = "{0}不能为空")]
        [StringLength(100, ErrorMessage = "{0}长度在2和100之间", MinimumLength = 2)]
        [Display(Name = "组织名称")]
        public string Name { get; set; }//组织名称
        [Required(ErrorMessage = "{0}不能为空")]
        [StringLength(10, ErrorMessage = "{0}长度在2和10之间", MinimumLength = 2)]
        [Display(Name = "组织编码")]
        public string Code { get; set; }//组织编码
        //public string Logo { get; set; }//组织logo
        public string Description { get; set; }
        [Required(ErrorMessage = "{0}不能为空")]
        [StringLength(20, ErrorMessage = "{0}长度在2和20之间", MinimumLength = 2)]
        [Display(Name = "用户名称")]
        public string Account { get; set; }//组织默认管理员
        [Required(ErrorMessage = "{0}不能为空")]
        [StringLength(20, ErrorMessage = "{0}长度在6和20之间", MinimumLength = 6)]
        [Display(Name = "密码")]
        public string Password { get; set; }
        [Required(ErrorMessage = "{0}不能为空")]
        [StringLength(20, ErrorMessage = "{0}长度在6和20之间", MinimumLength = 6)]
        [Display(Name = "密码")]
        public string PasswordAgain { get; set; }
    }
}
