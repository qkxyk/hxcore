using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using HXCloud.Model;

namespace HXCloud.Repository
{
    public interface ITypeHardwareConfigRepository : IBaseRepository<TypeHardwareConfigModel>
    {
        Task<TypeHardwareConfigModel> FindWithType(Expression<Func<TypeHardwareConfigModel, bool>> predicate);
    }
}
