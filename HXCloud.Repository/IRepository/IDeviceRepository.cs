using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HXCloud.Model;

namespace HXCloud.Repository
{
    public interface IDeviceRepository : IBaseRepository<DeviceModel>
    {
        Task AddAsync(DeviceModel entity, List<DeviceHardwareConfigModel> data);
    }
}
