using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using HXCloud.Model;

namespace HXCloud.Service
{
    public class DeviceHardwareConfigService : IDeviceHardwareConfigService
    {
        public Task<bool> IsExist(Expression<Func<DeviceHardwareConfigModel, bool>> predicate)
        {
            throw new NotImplementedException();
        }
    }
}
