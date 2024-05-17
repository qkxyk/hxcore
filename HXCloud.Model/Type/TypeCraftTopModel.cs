using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.Model
{
    /// <summary>
    /// 类型工艺top数据
    /// </summary>
    public class TypeCraftTopModel:BaseModel
    {
        public int Id { get; set; }//标识
        public string Name { get; set; }//top名称
        public string Url { get; set; }//top数据对应的url

        public int TypeId { get; set; }
        public virtual TypeModel Type { get; set; }
        public int Sn { get; set; } = 0;//数据序号
        public string Key { get; set; }//关键字，同一个类型不能重复
    }
}
