using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using HXCloud.Model;
using HXCloud.ViewModel;

namespace HXCloud.Service
{
    public interface ITypeConfigService : IBaseService<TypeConfigModel>
    {
        Task<BaseResponse> AddAsync(int typeId, TypeConfigAddViewModel req, string account);
        Task<BaseResponse> UpdateAsync(TypeConfigUpdateViewModel req, string account);
        Task<BaseResponse> DeleteAsync(int Id, string account);
        Task<BaseResponse> FindById(int Id);
        Task<BaseResponse> FindByType(int typeId, TypeConfigPageRequestViewModel req);
        bool IsExist(Expression<Func<TypeConfigModel, bool>> predicate, out string GroupId);
    }
}
