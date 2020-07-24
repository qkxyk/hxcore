using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using HXCloud.Model;
using HXCloud.ViewModel;

namespace HXCloud.Service
{
    public interface ITypeHardwareConfigService : IBaseService<TypeHardwareConfigModel>
    {
        Task<List<TypeHardwareConfigModel>> GetTypeHardwareConfigAsync(int TypeId);
        bool IsExist(Expression<Func<TypeHardwareConfigModel, bool>> predicate, out string GroupId);
        Task<BaseResponse> AddTypeHarewareConfigAsync(int typeId, TypeHardwareConfigAddDto req, string account);
        Task<BaseResponse> UpdateTypeHardwareConfigAsync(int typeId, TypeHardwareConfigUpdateDto req, string account);
        Task<BaseResponse> DeleteTypeHardwareConfigAsync(int Id, string account);
        Task<BaseResponse> GetHardwareConfigAsync(int Id);
        Task<BaseResponse> GetTypeHardwareConfigAsync(int typeId, BasePageRequest req);
    }
}
