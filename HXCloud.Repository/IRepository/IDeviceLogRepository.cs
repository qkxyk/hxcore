using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HXCloud.Model;

namespace HXCloud.Repository
{
    public interface IDeviceLogRepository : IBaseRepository<DeviceLogModel>
    {
        IEnumerable<DeviceLogData> GetWithUserNameAsync(List<DeviceLogModel> query);
    }
}
