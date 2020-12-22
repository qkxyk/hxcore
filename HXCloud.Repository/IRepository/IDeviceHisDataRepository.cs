using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using HXCloud.Model;

namespace HXCloud.Repository
{
    public interface IDeviceHisDataRepository : IBaseRepository<DeviceHisDataModel>
    {
        IQueryable<DeviceHisDataModel> FindWithDevice(Expression<Func<DeviceHisDataModel, bool>> lambda);
    }
}
