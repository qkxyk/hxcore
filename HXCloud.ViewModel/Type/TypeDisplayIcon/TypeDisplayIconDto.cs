using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.ViewModel
{
    /// <summary>
    /// 要显示相关的数据定义Key
    /// </summary>
    public class TypeDisplayIconDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Sn { get; set; }  //显示顺序
        public string DataDefineKey { get; set; }//关联的数据定义Key，目前只需要key，后续需要再添加
    }
}
