using HXCloud.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace HXCloud.Repository
{
    public interface IDeviceDiscreteStatisticsRepository : IBaseRepository<DeviceDiscreteStatisticsDataModel>
    {
        IQueryable<DeviceDiscreteStatisticsDataModel> FindWithDevice(Expression<Func<DeviceDiscreteStatisticsDataModel, bool>> lambda);
    }
}
