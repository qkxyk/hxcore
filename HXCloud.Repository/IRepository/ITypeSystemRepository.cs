using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using HXCloud.Model;

namespace HXCloud.Repository
{
    public interface ITypeSystemRepository : IBaseRepository<TypeSystemModel>
    {
        Task<TypeSystemModel> FindWithAccessory(Expression<Func<TypeSystemModel, bool>> predicate);
        Task<TypeSystemModel> FindWithType(Expression<Func<TypeSystemModel, bool>> predicate);
    }
}
