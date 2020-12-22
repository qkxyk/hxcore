using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using HXCloud.Model;
using Microsoft.EntityFrameworkCore;

namespace HXCloud.Repository
{
    public class DeviceStatisticsDataRepository : BaseRepository<DeviceStatisticsDataModel>, IDeviceStatisticsDataRepository
    {
        public IQueryable<DeviceStatisticsDataModel> FindWithDevice(Expression<Func<DeviceStatisticsDataModel, bool>> lambda)
        {
            var ret = _db.DeviceStatisticsData.Include(a=>a.Device).AsNoTracking().Where(lambda);
            return ret;
        }
    }
}
