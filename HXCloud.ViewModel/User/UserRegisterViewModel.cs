using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HXCloud.ViewModel
{
    public class UserRegisterViewModel
    {
        [Required(ErrorMessage = "账号名称必须输入")]
        [MaxLength(25, ErrorMessage = "账号长度不能超过25个字符")]
        [MinLength(6, ErrorMessage = "账号长度不能小于6个字符")]
        public string Account { get; set; }
        [Required(ErrorMessage = "密码必须输入")]
        [MaxLength(25, ErrorMessage = "密码长度不能超过25个字符")]
        [MinLength(6, ErrorMessage = "密码长度不能小于6个字符")]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "密码只能为字母和数组组成的字符串")]
        public string Password { get; set; }
        public string PasswordAgain { get; set; }
        public string Phone { get; set; }//联系电话
        public string UserName { get; set; }//用户姓名
        [Required(ErrorMessage = "组织编号不能为空")]
        public string GroupId { get; set; }
        //public string FirstName { get; set; }//用户姓名
        //public string LastName { get; set; }//用户名字
        //[RegularExpression(@"\w + ([-+.]\w +)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessage = "电子邮件地址不正确")]
        //public string Email { get; set; }//用户邮件
        //[Required(ErrorMessage = "组织代号不能为空")]
        //public string Code { get; set; }//组织代号
    }
}
