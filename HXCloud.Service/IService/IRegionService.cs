using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HXCloud.Model;
using HXCloud.ViewModel;

namespace HXCloud.Service
{
    public interface IRegionService : IBaseService<RegionModel>
    {
        Task<BaseResponse> AddRegionAsync(string account, string groupId, RegionAddDto req);
        Task<BaseResponse> UpdateRegionAsync(string account, string GroupId, RegionUpdateDto req);
        Task<BaseResponse> DeleteRegionAsync(string account, string Id, string GroupId);
        Task<BaseResponse> GetRegionAsync(string Id, string GroupId);
        Task<BaseResponse> GetRegionWithChildAsync(string Id, string GroupId);
        Task<BaseResponse> GetGroupRegionAsync(string GroupId);
    }
}
