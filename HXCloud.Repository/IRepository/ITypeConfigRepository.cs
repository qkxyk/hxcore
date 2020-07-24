using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using HXCloud.Model;

namespace HXCloud.Repository
{
    public interface ITypeConfigRepository : IBaseRepository<TypeConfigModel>
    {
        Task<TypeConfigModel> FindWithType(Expression<Func<TypeConfigModel, bool>> predicate);
    }
}
