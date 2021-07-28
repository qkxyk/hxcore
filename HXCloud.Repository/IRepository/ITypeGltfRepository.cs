using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using HXCloud.Model;

namespace HXCloud.Repository
{
    public interface ITypeGltfRepository : IBaseRepository<TypeGltfModel>
    {
        Task<TypeGltfModel> FindAsync(Expression<Func<TypeGltfModel, bool>> predicate);
    }
}
