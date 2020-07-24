using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HXCloud.Model;

namespace HXCloud.Repository
{
    public class DeviceRepository : BaseRepository<DeviceModel>, IDeviceRepository
    {
        public async Task AddAsync(DeviceModel entity, List<DeviceHardwareConfigModel> data)
        {
            //ef core默认的事务处理
            _db.Devices.Add(entity);
            _db.DeviceHardwareConfigs.AddRange(data);
            await _db.SaveChangesAsync();
        }
    }
}
