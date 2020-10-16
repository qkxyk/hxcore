using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.ViewModel
{
    public class RoleProjectDto
    {
        public int RoleId { get; set; }
        public int ProjectId { get; set; }
        public int Operate { get; set; }//0为查看，1为审核或者控制，2为编辑，3为添加或者删除
    }
}
