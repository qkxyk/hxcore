using HXCloud.Model;
using HXCloud.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HXCloud.Service
{
    public interface IDeviceDiscreteStatisticsService : IBaseService<DeviceDiscreteStatisticsDataModel>
    {
        Task<BaseResponse> GetDeviceStatisticsAsync(DeviceDisStatisticsRequestDto req, List<string> devices);
    }
}
