using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace HXCloud.ViewModel
{
    public class ProjectCheckDto
    {
        public bool IsExist { get; set; }     //项目或者场站是否存在
        public string PathId { get; set; }                          //项目的路径
        public string GroupId { get; set; }                                    //项目或者场站所属的组织
        public bool IsSite { get; set; }                                                                //是否为场站
    }
}
