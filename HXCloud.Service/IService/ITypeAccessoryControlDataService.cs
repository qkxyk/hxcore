using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using HXCloud.Model;
using HXCloud.ViewModel;

namespace HXCloud.Service
{
    public interface ITypeAccessoryControlDataService : IBaseService<TypeAccessoryControlDataModel>
    {
        bool IsExist(Expression<Func<TypeAccessoryControlDataModel, bool>> predicate, out string GroupId);
        Task<BaseResponse> AddAccessoryControlData(int accessoryId, TypeControlDataAddDto req, string account);
        Task<BaseResponse> UpdateTypeControlDataAsync(int accessoryId, TypeControlDataUpdateDto req, string account);
        Task<BaseResponse> DeleteTypeAccessoryControlDataAsync(int Id, string account);
        Task<BaseResponse> GetControlDataAsync(int Id);
        Task<BaseResponse> GetAccessoryControlDataAsync(int accessoryId);
    }
}
