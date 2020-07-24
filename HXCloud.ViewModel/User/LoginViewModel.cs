using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HXCloud.ViewModel
{
    //用户登录数据
    public class LoginViewModel
    {
        [Required(ErrorMessage = "{0}不能为空")]
        [Display(Name = "用户名")]
        public string Account { get; set; }
        [Required(ErrorMessage = "{0}不能为空")]
        [Display(Name = "密码")]
        public string Password { get; set; }

        public int Mark { get; set; } = 0;//pc端为0，移动端为1，让pc端和移动端同时登录
    }
}
