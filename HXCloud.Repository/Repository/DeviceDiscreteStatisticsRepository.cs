using HXCloud.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace HXCloud.Repository
{
    public class DeviceDiscreteStatisticsRepository : BaseRepository<DeviceDiscreteStatisticsDataModel>, IDeviceDiscreteStatisticsRepository
    {
        public IQueryable<DeviceDiscreteStatisticsDataModel> FindWithDevice(Expression<Func<DeviceDiscreteStatisticsDataModel, bool>> lambda)
        {
            var ret = _db.DeviceDiscreteStatisticsData.Include(a => a.Device).AsNoTracking().Where(lambda);
            return ret;
        }
    }
}
