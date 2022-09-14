using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HXCloud.Model;

namespace HXCloud.Repository
{
    public class DeviceLogRepository : BaseRepository<DeviceLogModel>, IDeviceLogRepository
    {
        //public dynamic GetWithUserNameAsync(Func<DeviceLogModel,bool> predicate)
        //{
        //   var data = _db.DeviceLogs.Where(predicate).Join(_db.Users, a => a.Account, b => b.Account, (a, b) => new { a, b.UserName }).AsQueryable();
        //    return data;
        //}

        public IEnumerable<DeviceLogData> GetWithUserNameAsync(List<DeviceLogModel> query)
        {
            var data = query.Join(_db.Users, a => a.Account, b => b.Account, (a, b) => new DeviceLogData
            {
                Id = a.Id,
                Account = a.Account,
                UserName = b.UserName,
                DeviceSn = a.DeviceSn,
                Key = a.Key,
                KeyName = a.KeyName,
                NewValue = a.NewValue,
                OldValue = a.OldValue,
                SendTime = a.SendTime,
                Time = a.Time,
                Value = a.Value
            });
            return data;
        }
    }
}
