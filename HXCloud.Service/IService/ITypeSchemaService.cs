using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using HXCloud.Model;
using HXCloud.ViewModel;

namespace HXCloud.Service
{
    public interface ITypeSchemaService : IBaseService<TypeSchemaModel>
    {
        Task<BaseResponse> AddSchemaAsync(int typeId, TypeSchemaAddViewModel req, string account);
        bool IsExist(Expression<Func<TypeSchemaModel, bool>> predicate, out string groupId);
        Task<BaseResponse> Delete(int Id, string account);
        Task<BaseResponse> Update(TypeSchemaUpdateViewModel req, string account);
        Task<BaseResponse> GetSchemaById(int Id);
        Task<BaseResponse> GetTypeSchema(int TypeId);
    }
}
