using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using HXCloud.Model;
using HXCloud.Repository;
using Microsoft.Extensions.Logging;

namespace HXCloud.Service
{
    public class DeviceHardwareConfigService : IDeviceHardwareConfigService
    {
        private readonly ILogger<DeviceHardwareConfigService> _log;
        private readonly IMapper _map;
        private readonly IDeviceHardwareConfigRepository _dhc;

        public DeviceHardwareConfigService(ILogger<DeviceHardwareConfigService> log, IMapper map, IDeviceHardwareConfigRepository dhc)
        {
            this._log = log;
            this._map = map;
            this._dhc = dhc;
        }
        public Task<bool> IsExist(Expression<Func<DeviceHardwareConfigModel, bool>> predicate)
        {
            throw new NotImplementedException();
        }
    }
}
