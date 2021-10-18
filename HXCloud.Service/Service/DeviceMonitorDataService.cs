using AutoMapper;
using HXCloud.Model;
using HXCloud.Repository;
using HXCloud.ViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HXCloud.Service
{
    public class DeviceMonitorDataService : IDeviceMonitorDataService
    {
        private readonly IDeviceMonitorDataRepository _dmdr;
        private readonly ILogger<DeviceMonitorDataService> _log;
        private readonly IMapper _mapper;

        public DeviceMonitorDataService(IDeviceMonitorDataRepository dmdr, ILogger<DeviceMonitorDataService> log, IMapper mapper)
        {
            this._dmdr = dmdr;
            this._log = log;
            this._mapper = mapper;
        }
        public Task<bool> IsExist(Expression<Func<DeviceMonitorDataModel, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取设备数采仪数据
        /// </summary>
        /// <param name="DeviceSn">设备序列号</param>
        /// <returns></returns>
        public async Task<BaseResponse> GetDeviceMonitorAsync(string DeviceSn, DeviceMonitorDataRequestDto req)
        {
            /*   DateTime Begin, End;
               if (req.Dt == null)
               {
                   Begin = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 00:00:00"));
                   End = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 23:59:59"));
               }
               else
               {
                   Begin = Convert.ToDateTime(req.Dt.Value.ToString("yyyy-MM-dd 00:00:00"));
                   End = Convert.ToDateTime(req.Dt.Value.ToString("yyyy-MM-dd 23:59:59"));
               }
               var query = _dmdr.Find(a => a.DeviceSn == DeviceSn&&a.Date>Begin&&a.Date<End);*/
            req.GetDate();//设置时间，如果dt有值，设置为某一天的开始和结束时间，否则就按输入的时间
            var query = _dmdr.Find(a => a.DeviceSn == DeviceSn && a.Date > req.Begin && a.Date < req.End);
            var data = await query.ToListAsync();
            var dtos = _mapper.Map<List<DeviceMonitorDto>>(data);
            return new BResponse<List<DeviceMonitorDto>>
            {
                Success = true,
                Message = "获取数据成功",
                Data = dtos
            };
        }
    }
}
