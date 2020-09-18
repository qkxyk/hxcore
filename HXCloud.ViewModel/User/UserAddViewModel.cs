using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace HXCloud.ViewModel
{
    public class UserAddViewModel
    {
        [Required(ErrorMessage = "账号名称必须输入")]
        [MaxLength(25, ErrorMessage = "账号长度不能超过25个字符")]
        [MinLength(6, ErrorMessage = "账号长度不能小于6个字符")]
        public string Account { get; set; }//用户账号
        [Required(ErrorMessage = "密码必须输入")]
        [MaxLength(25, ErrorMessage = "密码长度不能超过25个字符")]
        [MinLength(6, ErrorMessage = "密码长度不能小于6个字符")]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "密码只能为字母和数组组成的字符串")]
        public string Password { get; set; }//用户密码
        [Required(ErrorMessage = "密码必须输入")]
        [MaxLength(25, ErrorMessage = "密码长度不能超过25个字符")]
        [MinLength(6, ErrorMessage = "密码长度不能小于6个字符")]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "密码只能为字母和数组组成的字符串")]
        public string PasswordAgain { get; set; }//用户密码
        public string Phone { get; set; }//联系电话
        [Required(ErrorMessage = "用户名称不能为空")]
        public string UserName { get; set; }
        [RegularExpression(@"^[a-zA-Z0-9_-]+@[a-zA-Z0-9_-]+(\.[a-zA-Z0-9_-]+)+$", ErrorMessage = "电子邮件地址不正确")]
        public string Email { get; set; }//用户邮件
        //public int Status { get; set; }//用户状态,未激活、有效用户、无效用户
    }
}
