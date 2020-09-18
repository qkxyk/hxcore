using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.ViewModel
{
    public class UserInfoDto
    {
        public int Id { get; set; }//用户标示
        public string Account { get; set; }//用户账号
        public string Phone { get; set; }//联系电话
        public string UserName { get; set; }
        public DateTime Create { get; set; }//用户创建时间
        public DateTime LastLogin { get; set; }//上次登录时间
        public string Email { get; set; }//用户邮件
        public string Picture { get; set; }
        public int Status { get; set; }//用户状态,未激活、有效用户、无效用户
        public string GroupId { get; set; }
        public string GroupName { get; set; }
        public string Code { get; set; }
        public string Logo { get; set; }
        public bool IsAdmin { get; set; }
    }
}
