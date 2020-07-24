using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using HXCloud.Model;
using HXCloud.Repository;
using HXCloud.ViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace HXCloud.Service
{
    public class AppVersionService : IAppVersionService
    {
        private readonly ILogger<AppVersionService> _log;
        private readonly IAppVersionRepository _avr;
        private readonly IMapper _mapper;

        public AppVersionService(ILogger<AppVersionService> log, IAppVersionRepository avr, IMapper mapper)
        {
            this._log = log;
            this._avr = avr;
            this._mapper = mapper;
        }
        public async Task<bool> IsExist(Expression<Func<AppVersionModel, bool>> predicate)
        {
            var d = await _avr.Find(predicate).FirstOrDefaultAsync();
            if (d == null)
            {
                return false;
            }
            return true;
        }
        public async Task<BaseResponse> AddAppVersionAsync(string account, string path, AppVersionAddDto req)
        {
            //验证版本号是否相同
            var app = await _avr.Find(a => a.VersionNo == req.VersionNo).FirstOrDefaultAsync();
            if (app != null)
            {
                return new BaseResponse { Success = false, Message = "已存在相同版本号的升级文件" };
            }
            try
            {
                var entity = _mapper.Map<AppVersionModel>(req);
                entity.Create = account;
                await _avr.AddAsync(entity);
                _log.LogInformation($"{account}添加标示为{entity.Id}升级文件成功");
                return new HandleResponse<int> { Success = true, Message = "添加文件成功", Key = entity.Id };
            }
            catch (Exception ex)
            {
                _log.LogError($"{account}添加升级文件{req.VersionNo}失败，失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "添加失败文件失败" };
            }
        }

        public async Task<BaseResponse> UpdateAppVersionAsync(string account, AppVersionUpdateDto req)
        {
            //检查是否存在相同的版本号
            var ext = await _avr.Find(a => a.VersionNo == req.VersionNo && a.Id != req.Id).FirstOrDefaultAsync();
            if (ext != null)
            {
                return new BaseResponse { Success = false, Message = "已存在相同版本号的升级文件" };
            }
            var entity = await _avr.FindAsync(req.Id);
            if (entity == null)
            {
                return new BaseResponse { Success = false, Message = "输入的升级文件编号不存在" };
            }
            try
            {
                _mapper.Map(req, entity);
                entity.Modify = account;
                entity.ModifyTime = DateTime.Now;
                await _avr.SaveAsync(entity);
                _log.LogInformation($"{account}修改Id为{req.Id}的升级文件成功");
                return new BaseResponse { Success = true, Message = "修改升级文件成功" };
            }
            catch (Exception ex)
            {
                _log.LogError($"{account}修改Id为{req.Id}的升级文件失败,失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "修改升级文件失败，请联系管理员" };
            }
        }

        public async Task<BaseResponse> DeleteAppVersionAsync(string account, int Id, string path)
        {
            var ret = await _avr.FindAsync(Id);
            if (ret == null)
            {
                return new BaseResponse { Success = false, Message = "输入的文件编号不存在" };
            }
            try
            {
                //先删除文件
                string url = Path.Combine(path, ret.Address);
                if (System.IO.File.Exists(url))
                {
                    System.IO.File.Delete(url);
                }
                await _avr.RemoveAsync(ret);
                _log.LogInformation($"{account}删除文件{Id}成功");
                return new BaseResponse { Success = true, Message = "删除文件成功" };
            }
            catch (Exception ex)
            {
                _log.LogError($"{account}删除文件{Id}失败，失败原因{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "删除文件失败，请联系管理员" };
            }
        }
        public async Task<BaseResponse> GetAppVersionAsync(int Id)
        {
            var ret = await _avr.FindAsync(Id);
            if (ret == null)
            {
                return new BaseResponse { Success = false, Message = "输入的升级文件标示不存在" };
            }
            var dto = _mapper.Map<AppVersionDto>(ret);
            var rm = new BResponse<AppVersionDto> { Success = true, Message = "获取数据成功", Data = dto };
            return rm;
        }
        public async Task<BaseResponse> GetPageAppVersionAsync(BasePageRequest req)
        {
            var query = _avr.Find(a => true == true);
            if (!string.IsNullOrWhiteSpace(req.Search))
            {
                query = query.Where(a => a.VersionNo.Contains(req.Search));
            }
            int Count = query.Count();
            string OrderExpression = "";
            if (string.IsNullOrEmpty(req.OrderBy))
            {
                OrderExpression = "Id Asc";
                //UserQuery = UserQuery.OrderBy(a => a.Id);
            }
            else
            {
                OrderExpression = string.Format("{0} {1}", req.OrderBy, req.OrderType);
            }
            var data = await query.OrderBy(OrderExpression).Skip((req.PageNo - 1) * req.PageSize).Take(req.PageSize).ToListAsync();
            var dtos = _mapper.Map<List<AppVersionDto>>(data);
            return new BasePageResponse<List<AppVersionDto>>
            {
                Success = true,
                Message = "获取数据成功",
                Count = Count,
                CurrentPage = req.PageNo,
                PageSize = req.PageSize,
                TotalPage = (int)Math.Ceiling((decimal)Count / req.PageSize),
                Data = dtos
            };
        }
    }
}
