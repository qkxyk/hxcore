using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.ViewModel
{
    public class TypeCraftTopExistDto
    {
        public bool IsExist { get; set; }//是否存在
        public string Url { get; set; }//文件地址
        public string Account { get; set; }//拓扑数据上传者
    }
}
