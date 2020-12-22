using System;
using System.Collections.Generic;
using System.IO;
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
            //添加设备的迁移记录
            _db.DeviceMigrations.Add(new DeviceMigrationModel { Create = entity.Create, CreateTime = entity.CreateTime, CurrentPId = entity.ProjectId, DeviceNo = entity.DeviceNo, DeviceSn = entity.DeviceSn, GroupId = entity.GroupId, TypeId = 0 });
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

        /// <summary>
        /// 迁移设备
        /// </summary>
        /// <param name="entity">设备信息</param>
        /// <param name="migration">设备迁移信息</param>
        /// <returns></returns>
        public async Task SaveDeviceWithMigrationAsync(DeviceModel entity, DeviceMigrationModel migration)
        {
            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    _db.Entry<DeviceModel>(entity).State = EntityState.Modified;
                    _db.DeviceMigrations.Add(migration);
                    await _db.SaveChangesAsync();
                    await trans.CommitAsync();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
            }

        }
        public async Task<bool> DeleteAsync(string DeviceSn)
        {
            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    //设备删除之前，先删除设备的一些相关数据
                    var device = await _db.Devices.Where(a => a.DeviceSn == DeviceSn).FirstOrDefaultAsync();
                    if (device == null)
                    {
                        return false;
                    }
                    var statistics = await _db.DeviceStatisticsData.Where(a => a.DeviceSn == DeviceSn).ToListAsync();
                    _db.DeviceStatisticsData.RemoveRange(statistics);
                    var discreteStatistics = await _db.DeviceDiscreteStatisticsData.Where(a => a.DeviceSn == DeviceSn).ToListAsync();
                    _db.DeviceDiscreteStatisticsData.RemoveRange(discreteStatistics);
                    var his = await _db.DeviceHisData.Where(a => a.DeviceSn == DeviceSn).ToListAsync();
                    _db.DeviceHisData.RemoveRange(his);
                    var card = await _db.DeviceCards.Where(a => a.DeviceSn == DeviceSn).ToListAsync();
                    _db.DeviceCards.RemoveRange(card);
                    var config = await _db.DeviceConfigs.Where(a => a.DeviceSn == DeviceSn).ToListAsync();
                    _db.DeviceConfigs.RemoveRange(config);
                    var hard = await _db.DeviceHardwareConfigs.Where(a => a.DeviceSn == DeviceSn).ToListAsync();
                    _db.DeviceHardwareConfigs.RemoveRange(hard);
                    var log = await _db.DeviceLogs.Where(a => a.DeviceSn == DeviceSn).ToListAsync();
                    _db.DeviceLogs.RemoveRange(log);
                    var migration = await _db.DeviceMigrations.Where(a => a.DeviceSn == DeviceSn).ToListAsync();
                    _db.DeviceMigrations.RemoveRange(migration);
                    var online = await _db.DeviceOnlines.Where(a => a.DeviceSn == DeviceSn).ToListAsync();
                    _db.DeviceOnlines.RemoveRange(online);
                    var video = await _db.DeviceVideos.Where(a => a.DeviceSn == DeviceSn).ToListAsync();
                    _db.DeviceVideos.RemoveRange(video);
                    var img = await _db.DeviceImages.Where(a => a.DeviceSn == DeviceSn).ToListAsync();
                    _db.DeviceImages.RemoveRange(img);
                    _db.Devices.Remove(device);
                    await _db.SaveChangesAsync();
                    await trans.CommitAsync();
                    return true;
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
            }
        }
    }
}
