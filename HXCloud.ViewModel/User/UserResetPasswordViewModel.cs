using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HXCloud.ViewModel
{
    public class UserResetPasswordViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "密码必须输入")]
        [MaxLength(25, ErrorMessage = "密码长度不能超过25个字符")]
        [MinLength(6, ErrorMessage = "密码长度不能小于6个字符")]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "密码只能为字母和数组组成的字符串")]
        public string Password { get; set; }
    }
}
