using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using HXCloud.Model;

namespace HXCloud.Repository
{
    public interface IDeviceHisDataRepository //: IBaseRepository<DeviceHisDataModel>
    {
        Task<IEnumerable<CoreDeviceHisDataMoel>> FindDeviceHisDataAsync(string TableName, string DeviceSn);

        Task<CoreDeviceHisDataMoel> FindFirstOrDefaultAsync(string TableName, string DeviceSn, string orderType);
        Task<Boolean> CheckTableExistAsyn(string TableName);
    }
}
