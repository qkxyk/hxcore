using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using HXCloud.Model;

namespace HXCloud.Repository
{
    public interface ITypeAccessoryControlDataRepository : IBaseRepository<TypeAccessoryControlDataModel>
    {
        Task<TypeAccessoryControlDataModel> FindWithType(Expression<Func<TypeAccessoryControlDataModel, bool>> predicate);
    }
}
