using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using HXCloud.Model;

namespace HXCloud.Repository
{
    public interface ITypeSystemAccessoryRepository : IBaseRepository<TypeSystemAccessoryModel>
    {
        Task<TypeSystemAccessoryModel> FindWithSystem(Expression<Func<TypeSystemAccessoryModel, bool>> predicate);
        Task<TypeSystemAccessoryModel> FindWithControlData(Expression<Func<TypeSystemAccessoryModel, bool>> predicate);
    }
}
