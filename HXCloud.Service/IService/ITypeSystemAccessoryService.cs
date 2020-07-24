using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using HXCloud.Model;
using HXCloud.ViewModel;

namespace HXCloud.Service
{
    public interface ITypeSystemAccessoryService : IBaseService<TypeSystemAccessoryModel>
    {
        bool IsExist(Expression<Func<TypeSystemAccessoryModel, bool>> predicate, out string GroupId);
        Task<BaseResponse> AddSystemAccessoryAsync(int systemId, TypeSystemAccessoryAddDto req, string account);
        Task<BaseResponse> UpdateTypeSystemAccessoryAsync(int systemId, TypeSystemAccessoryUpdateDto req, string account);
        Task<BaseResponse> DeleteSystemAccessoryAsync(int Id, string account);
        Task<BaseResponse> GetAccessoryAsync(int Id);
        Task<BaseResponse> GetSystemAccessory(int systemId, BasePageRequest req);
    }
}
