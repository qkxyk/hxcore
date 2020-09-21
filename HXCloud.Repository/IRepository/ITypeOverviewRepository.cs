using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using HXCloud.Model;

namespace HXCloud.Repository
{
    public interface ITypeOverviewRepository : IBaseRepository<TypeOverviewModel>
    {
        /// <summary>
        /// 获取类型总揽数据，以及对应的类型数据定义
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<IEnumerable<TypeOverviewModel>> FindWithDataDefine(Expression<Func<TypeOverviewModel, bool>> predicate);
    }
}
