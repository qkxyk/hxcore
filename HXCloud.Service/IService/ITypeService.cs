using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using HXCloud.Model;
using HXCloud.ViewModel;

namespace HXCloud.Service
{
    public interface ITypeService : IBaseService<TypeModel>
    {
        Task<BaseResponse> AddType(TypeAddViewModel req, string account);
        Task<BaseResponse> UpdateType(TypeUpdateViewModel req, string account);
        Task<string> GetTypeGroupIdAsync(int Id);
        Task<BaseResponse> DeleteTypeAsync(int Id, string account);
        Task<BaseResponse> GetGroupTypeAsync(string GroupId);
        Task<BaseResponse> GetTypeAsync(int Id);
        Task<TypeCheckDto> CheckTypeAsync(int Id);
        bool IsExist(int Id, out string GroupId);
        bool IsExist(Expression<Func<TypeModel, bool>> predicate, out string GroupId);
    }
}
