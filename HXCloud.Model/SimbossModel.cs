using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.Model
{
    /// <summary>
    /// sim卡账号管理
    /// </summary>
   public class SimbossModel:BaseModel
    {
        public int Id { get; set; }
        public string SimAccount { get; set; }//simboss登录账号
        public string SimPassword { get; set; }//simboss登录密码
        public string  AppId { get; set; }//simboss开发者id
        public string AppSecret { get; set; }//simboss开发者密钥
    }
}
