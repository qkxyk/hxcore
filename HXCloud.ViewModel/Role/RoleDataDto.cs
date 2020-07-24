using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.ViewModel
{
    public class RoleDataDto
    {
        public int Id { get; set; } //角色标示
        public string Name { get; set; }//角色名称
        public string Description { get; set; }//角色描述
        public string Edit { get; set; }
        public DateTime EditTime { get; set; }
        public string Admin { get; set; }
    }
}
