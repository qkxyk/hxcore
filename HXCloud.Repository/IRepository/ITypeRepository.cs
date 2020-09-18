using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using HXCloud.Model;

namespace HXCloud.Repository
{
    public interface ITypeRepository : IBaseRepository<TypeModel>
    {
        Task<TypeModel> FindAsync(Expression<Func<TypeModel, bool>> predicate);
        /// <summary>
        /// 拷贝类型
        /// </summary>
        /// <param name="sourceId">源类型标示</param>
        /// <param name="target">目标类型</param>
        /// <returns></returns>
        Task CopyTypeAsync(int sourceId, TypeModel target);
    }
}
