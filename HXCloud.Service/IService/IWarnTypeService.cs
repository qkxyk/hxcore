using HXCloud.Model;
using HXCloud.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HXCloud.Service
{
    public interface IWarnTypeService : IBaseService<WarnTypeModel>
    {
        Task<BaseResponse> AddWarnTypeAsync(string account, WarnTypeAddDto req);
        Task<BaseResponse> ModifyWarnTypeAsync(string account, WarnTypeUpdateDto req);
        Task<BaseResponse> DeleteWarnTypeAsync(string account, int Id);
        Task<BaseResponse> FindWarnTypeByIdAsync(int Id);
        Task<BaseResponse> FindWarnTypeAsync();
    }
    public interface IWarnCodeService : IBaseService<WarnCodeModel>
    {
        Task<BaseResponse> AddWarnCodeAsync(string account, int warnTypeId, WarnCodeAddDto req);
        Task<BaseResponse> UpdateWarnCodeDescriptionAsync(string account, string code, string description);
        Task<BaseResponse> DeleteWarnCodeAsync(string account, string code);
        Task<BaseResponse> GetPageWarnCodeAsync(WarnCodePageRequest req);
    }
    public interface IWarnService : IBaseService<WarnModel>
    {
        Task<BaseResponse> GetWarnById(int Id);
        Task<BaseResponse> GetWarnByDeviceSnAsync(string deviceSn, DeviceWarnPageRequest req);
        Task<BaseResponse> GetProjectWarnAsync(List<string> Devices, DateTime begin, DateTime end, bool state, BasePageRequest req);
        Task<BaseResponse> GetWarnStatisticsAsync(List<string> Devices);
        Task<BaseResponse> UpdateWarnInfo(string account, WarnUpdateDto req);
    }
}
