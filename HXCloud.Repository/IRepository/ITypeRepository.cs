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
    }
}
