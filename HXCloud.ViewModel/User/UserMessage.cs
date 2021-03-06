﻿using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.ViewModel
{
    //用户登录后需要序列化的数据
    public class UserMessage
    {
        public string Account { get; set; }
        public bool IsAdmin { get; set; }
        public int Id { get; set; }
        public string UserName { get; set; }
        public string GroupName { get; set; }
        public string GroupId { get; set; }
        public string Code { get; set; }
        public string Roles { get; set; }
        public DateTime Dt { get; set; }
        public int UserStatus { get; set; }//用户状态,未激活、有效用户、无效用户态
        public string Key { get; set; }//redis保存的key值
    }
}
