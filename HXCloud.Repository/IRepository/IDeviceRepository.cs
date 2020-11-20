﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using HXCloud.Model;

namespace HXCloud.Repository
{
    public interface IDeviceRepository : IBaseRepository<DeviceModel>
    {
        Task AddAsync(DeviceModel entity, List<DeviceHardwareConfigModel> data);
        Task SaveAsync(DeviceModel entity, List<DeviceHardwareConfigModel> data);
        IQueryable<DeviceModel> FindWithOnlineAndImages(Expression<Func<DeviceModel, bool>> predicate);
        /// <summary>
        /// 获取设备和设备在线信息
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <returns></returns>
        IQueryable<DeviceModel> FindWithOnline(Expression<Func<DeviceModel, bool>> predicate);
    }
}
