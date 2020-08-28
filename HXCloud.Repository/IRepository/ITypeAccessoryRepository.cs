using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using HXCloud.Model;

namespace HXCloud.Repository
{
    public interface ITypeAccessoryRepository : IBaseRepository<TypeAccessoryModel>
    {
        Task<TypeAccessoryModel> FindWithType(Expression<Func<TypeAccessoryModel, bool>> predicate);
        Task<IEnumerable<TypeAccessoryModel>> FindWithControlData(int typeId);
    }
}
