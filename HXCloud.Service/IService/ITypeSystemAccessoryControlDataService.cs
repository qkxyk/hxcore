using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HXCloud.Model;
using HXCloud.ViewModel;

namespace HXCloud.Service
{
    public interface ITypeSystemAccessoryControlDataService : IBaseService<TypeSystemAccessoryControlDataModel>
    {
        Task<BaseResponse> AddSystemControlDataAsync(int accessoryId, TypeSystemAccessoryControlDataAddDto req, string account);
        Task<BaseResponse> UpdateSystemAccessoryControlDataAsync(int accessoryId, TypeSystemAccessoryControlDataUpdateDto req, string account);
        Task<BaseResponse> DeleteSystemAccessoryControlDataAsync(int Id, string account);
        Task<BaseResponse> GetControlDataAsync(int Id);
        Task<BaseResponse> GetAccessoryControlDataAsync(int accessoryId);
    }
}
