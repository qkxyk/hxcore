using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using HXCloud.Model;

namespace HXCloud.Service
{
    public class DeviceStatisticsDataService : IDeviceStatistcsDataService
    {
        public Task<bool> IsExist(Expression<Func<DeviceStatisticsDataModel, bool>> predicate)
        {
            throw new NotImplementedException();
        }
    }
}
