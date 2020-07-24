using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using HXCloud.Model;
using HXCloud.ViewModel;

namespace HXCloud.Service
{
    public interface ITypeSystemService : IBaseService<TypeSystemModel>
    {
        bool IsExist(Expression<Func<TypeSystemModel, bool>> predicate, out string GroupId);
        Task<BaseResponse> AddTypeSystemAsync(int typeId, TypeSystemAddDto req, string account);
        Task<BaseResponse> UpdateTypeSystemAsync(int typeId, TypeSystemUpdateDto req, string account);
        Task<BaseResponse> DeleteTypeSystemAsync(int Id, string account);
        Task<BaseResponse> GetSystemAsync(int Id);
        Task<BaseResponse> GetTypeSystemAsync(int typeId);
    }
}
