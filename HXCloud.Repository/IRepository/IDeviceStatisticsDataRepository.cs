using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using HXCloud.Model;

namespace HXCloud.Repository
{
    public interface IDeviceStatisticsDataRepository : IBaseRepository<DeviceStatisticsDataModel>
    {
        IQueryable<DeviceStatisticsDataModel> FindWithDevice(Expression<Func<DeviceStatisticsDataModel, bool>> lambda);
    }
}
