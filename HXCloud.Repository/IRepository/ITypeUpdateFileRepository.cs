using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using HXCloud.Model;

namespace HXCloud.Repository
{
    public interface ITypeUpdateFileRepository : IBaseRepository<TypeUpdateFileModel>
    {
        Task<TypeUpdateFileModel> GetTypeUpdateFileWithTypeAsync(Expression<Func<TypeUpdateFileModel, bool>> predicate);
    }
}
