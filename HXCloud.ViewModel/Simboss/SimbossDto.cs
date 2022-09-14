using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.ViewModel
{
    //simboss账号信息
    public class SimbossDto
    {
        public int Id { get; set; }
        public string SimAccount { get; set; }//simboss登录账号
        public string SimPassword { get; set; }//simboss登录密码
        public string AppId { get; set; }//simboss开发者id
        public string AppSecret { get; set; }//simboss开发者密钥
    }
}
