using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using HXCloud.Model;

namespace HXCloud.Repository
{
    public interface ITypeSchemaRepository : IBaseRepository<TypeSchemaModel>
    {
        Task<TypeSchemaModel> FindWithType(Expression<Func<TypeSchemaModel, bool>> lambda);
        Task<TypeSchemaModel> FindWithChild(int Id);
    }
}
