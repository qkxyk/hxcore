using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HXCloud.ViewModel
{
    //修改simboss账号信息
  public  class SimbossUpdateDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string SimAccount { get; set; }//simboss登录账号
        public string SimPassword { get; set; }//simboss登录密码
        [Required]
        public string AppId { get; set; }//simboss开发者id
        [Required]
        public string AppSecret { get; set; }//simboss开发者密钥
        public override string ToString()
        {
            return $"Id={Id}:simAccount={SimAccount}:simpassword={SimPassword}:appid={AppId}:appsecret={AppSecret}";
        }
    }
}
