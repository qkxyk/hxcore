using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using HXCloud.Model;
using Microsoft.EntityFrameworkCore;

namespace HXCloud.Repository
{
    public class DeviceRepository : BaseRepository<DeviceModel>, IDeviceRepository
    {
        public async Task AddAsync(DeviceModel entity, List<DeviceHardwareConfigModel> data)
        {
            //ef core默认的事务处理
            _db.Devices.Add(entity);
            _db.DeviceHardwareConfigs.AddRange(data);
            await _db.SaveChangesAsync();
        }
        public async Task SaveAsync(DeviceModel entity, List<DeviceHardwareConfigModel> data)
        {
            //先删除现有的硬件配置数据
            var dh = _db.DeviceHardwareConfigs.Where(a => a.DeviceSn == entity.DeviceSn).ToList();
            _db.DeviceHardwareConfigs.RemoveRange(dh);
            _db.Entry<DeviceModel>(entity).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _db.DeviceHardwareConfigs.AddRange(data);
            await _db.SaveChangesAsync();
        }

        public IQueryable<DeviceModel> FindWithOnlineAndImages(Expression<Func<DeviceModel, bool>> predicate)
        {
            var data = _db.Devices.Include(a => a.DeviceOnline).Include(a => a.DeviceImage).Where(predicate);
            return data;
        }
        /// <summary>
        /// 获取设备和设备在线信息
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <returns></returns>
        public IQueryable<DeviceModel> FindWithOnline(Expression<Func<DeviceModel, bool>> predicate)
        {
            var data = _db.Devices.Include(a => a.DeviceOnline).Where(predicate);
            return data;
        }
    }
}
