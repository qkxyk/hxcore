using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using HXCloud.Model;

namespace HXCloud.Repository
{
    public interface ITypeImageRepository : IBaseRepository<TypeImageModel>
    {
        Task<TypeImageModel> GetTypeImageWithTypeAsync(Expression<Func<TypeImageModel,bool>> predicate);
    }
}
