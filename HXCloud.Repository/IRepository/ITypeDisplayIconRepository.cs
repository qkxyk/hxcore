using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using HXCloud.Model;

namespace HXCloud.Repository
{
    public interface ITypeDisplayIconRepository : IBaseRepository<TypeDisplayIconModel>
    {
        /// <summary>
        /// 获取类型显示数据，以及相对应的类型数据定义
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<IEnumerable<TypeDisplayIconModel>> FindWithDataDefine(Expression<Func<TypeDisplayIconModel, bool>> predicate);
    }
}
