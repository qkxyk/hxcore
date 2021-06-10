using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HXCloud.ViewModel
{
    //用户只能修改自己的联系电话，用户真实姓名，电子邮件
    public class UserUpdateViewModel
    {
        public string Phone { get; set; }//联系电话
        public string UserName { get; set; }

        public string Email { get; set; }//用户邮件
        }
}
