using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.ViewModel
{
    /// <summary>
    /// 用于选择运维人员
    /// </summary>
    public class OpsUserDto
    {
        public string Account { get; set; }//用户账号
        public string UserName { get; set; }//用户名+用户的职位
    }
}
