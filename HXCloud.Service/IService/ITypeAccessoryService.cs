using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using HXCloud.Model;
using HXCloud.ViewModel;

namespace HXCloud.Service
{
    public interface ITypeAccessoryService : IBaseService<TypeAccessoryModel>
    {
        bool IsExist(Expression<Func<TypeAccessoryModel, bool>> predicate, out string GroupId);
        Task<BaseResponse> AddAccessoryAsync(int typeId, TypeAccessoryAddViewModel req, string account);
        Task<BaseResponse> UpdateTypeAccessoryAsync(int typeId, TypeAccessoryUpdateViewModel req, string account);
        Task<BaseResponse> DeleteTypeAccessoryAsync(int Id, string account);
        Task<BaseResponse> GetAccessoryAsync(int Id);
        Task<BaseResponse> GetTypeAccessoryAsync(int typeId, BasePageRequest req);
        Task<BaseResponse> GetTypeAccessoryAndControlDataAsync(int TypeId);
    }
}
