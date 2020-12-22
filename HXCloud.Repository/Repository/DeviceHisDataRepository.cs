using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using HXCloud.Model;
using Microsoft.EntityFrameworkCore;

namespace HXCloud.Repository
{
    public class DeviceHisDataRepository:BaseRepository<DeviceHisDataModel>,IDeviceHisDataRepository
    {
        public IQueryable<DeviceHisDataModel> FindWithDevice(Expression<Func<DeviceHisDataModel, bool>> lambda)
        {
            var ret = _db.DeviceHisData.Include(a => a.Device).AsNoTracking().Where(lambda);
            return ret;
        }
    }
}
