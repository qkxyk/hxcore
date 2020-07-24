using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using HXCloud.Model;

namespace HXCloud.Repository
{
    public interface ITypeDataDefineRepository : IBaseRepository<TypeDataDefineModel>
    {
        Task<TypeDataDefineModel> GetTypeDataDefineWithTypeAsync(Expression<Func<TypeDataDefineModel, bool>> predicate);
    }
}
