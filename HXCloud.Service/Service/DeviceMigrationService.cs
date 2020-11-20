using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using HXCloud.Model;
using HXCloud.Repository;
using HXCloud.ViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HXCloud.Service
{
    public class DeviceMigrationService : IDeviceMigrationService
    {
        private readonly IDeviceMigrationRepository _dmr;
        private readonly IMapper _mapper;

        public DeviceMigrationService(IDeviceMigrationRepository dmr, IMapper mapper)
        {
            this._dmr = dmr;
            this._mapper = mapper;
        }
        public Task<bool> IsExist(Expression<Func<DeviceMigrationModel, bool>> predicate)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 获取设备的迁移记录
        /// </summary>
        /// <param name="DeviceSn">设备序列号</param>
        /// <returns>返回设备的迁移记录</returns>
        public async Task<BaseResponse> GetDeviceMigrationAsync(string DeviceSn)
        {
            var data = await _dmr.Find(a => a.DeviceSn == DeviceSn).ToListAsync();
            var dtos = _mapper.Map<List<DeviceMigrationDto>>(data);
            return new BResponse<List<DeviceMigrationDto>> { Success = true, Message = "获取数据成功", Data = dtos };
        }

    }
}
