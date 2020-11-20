using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using HXCloud.Model;

namespace HXCloud.Repository
{
    public interface ITypeArgumentRepository : IBaseRepository<TypeArgumentModel>
    {
        Task<TypeArgumentModel> FindWithType(Expression<Func<TypeArgumentModel, bool>> predicate);
        Task<TypeArgumentModel> FindWithDataDefine(Expression<Func<TypeArgumentModel, bool>> predicate);
        IQueryable<TypeArgumentModel> FindWithDataDefines(Expression<Func<TypeArgumentModel, bool>> predicate);
    }
}
