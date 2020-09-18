using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.Model
{
    /// <summary>
    /// 分类数据定义，即给数据定义打上标签，方便查找
    /// </summary>
    public class CategoryModel : BaseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
