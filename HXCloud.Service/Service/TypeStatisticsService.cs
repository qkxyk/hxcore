using System;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
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
    public class TypeStatisticsService : ITypeStatisticsService
    {
        private readonly ILogger<TypeService> _log;
        private readonly IMapper _mapper;
        private readonly ITypeDataDefineRepository _tdd;
        private readonly ITypeStatisticsRepository _tsr;
        private readonly ITypeRepository _tr;

        public TypeStatisticsService(ILogger<TypeService> log, IMapper mapper, ITypeDataDefineRepository tdd, ITypeStatisticsRepository tsr, ITypeRepository tr)
        {
            this._log = log;
            this._mapper = mapper;
            this._tdd = tdd;
            this._tsr = tsr;
            this._tr = tr;
        }
        public async Task<bool> IsExist(Expression<Func<TypeStatisticsInfoModel, bool>> predicate)
        {
            var data = await _tsr.Find(predicate).FirstOrDefaultAsync();
            if (data == null)
            {
                return false;
            }
            return true;
        }
        public bool IsExist(Expression<Func<TypeStatisticsInfoModel, bool>> predicate, out string GroupId)
        {
            var data = _tsr.FindWithType(predicate).Result;
            if (data == null)
            {
                GroupId = null;
                return false;
            }
            GroupId = data.Type.GroupId;
            return true;
        }
        public async Task<BaseResponse> AddStatistics(int typeId, TypeStatisticsAddViewModel req, string account)
        {
            //验证类型是否可以添加
            var t = await _tr.FindAsync(typeId);
            if (t.Status == TypeStatus.Root)
            {
                return new BaseResponse { Success = false, Message = "目录节点类型不能添加具体数据" };
            }
            //验证数据定义是否存在
            var data = await _tdd.FindAsync(req.DataDefineId);
            if (data == null)
            {
                return new BaseResponse { Success = false, Message = "输入类型的数据定义不存在" };
            }
            //验证统计名称和数据定义key是否存在
            var count = _tsr.Find(a => a.TypeId == typeId && (a.Name == req.Name || a.DataKey == data.DataKey)).Count();
            if (count > 0)
            {
                return new BaseResponse { Success = false, Message = "此类型下已存在相同名称的统计数据或者相同的key值" };
            }
            try
            {
                var entity = _mapper.Map<TypeStatisticsInfoModel>(req);
                entity.TypeId = typeId;
                entity.Create = account;
                entity.DataKey = data.DataKey;
                entity.SUnit = data.Unit;
                await _tsr.AddAsync(entity);
                _log.LogInformation($"{account}添加标示为{entity.Id}的类型统计数据成功");
                return new HandleResponse<int> { Success = true, Message = "添加类型统计数据成功", Key = entity.Id };
            }
            catch (Exception ex)
            {
                _log.LogError($"{account}添加类型统计数据失败，失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "添加类型统计数据失败，请联系管理员" };
            }
        }
        public async Task<BaseResponse> UpdateStatistics(TypeStatisticsUpdateViewModel req, string account)
        {
            var data = await _tsr.FindAsync(req.Id);
            if (data == null)
            {
                return new BaseResponse { Success = false, Message = "输入的类型统计数据不存在" };
            }
            var ret = await _tsr.Find(a => a.Name == req.Name && a.TypeId == data.TypeId).FirstOrDefaultAsync();
            if (ret != null && ret.Id != req.Id)
            {
                return new BaseResponse { Success = false, Message = "已存在相同名称的类型统计数据" };
            }
            try
            {
                var entity = _mapper.Map(req, data);
                entity.Modify = account;
                entity.ModifyTime = DateTime.Now;
                await _tsr.SaveAsync(entity);
                _log.LogInformation($"{account}修改标示为{req.Id}的类型统计数据成功");
                return new BaseResponse { Success = true, Message = "修改类型统计数据成功" };
            }
            catch (Exception ex)
            {
                _log.LogError($"{account}修改标示为{req.Id}的类型统计数据失败,失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "修改类型统计数据失败，请联系管理员" };
            }
        }

        public async Task<BaseResponse> DeleteAsync(int Id, string account)
        {
            var data = await _tsr.FindAsync(Id);
            if (data == null)
            {
                return new BaseResponse { Success = false, Message = "输入的类型统计数据不存在" };
            }
            try
            {
                await _tsr.RemoveAsync(data);
                _log.LogInformation($"{account}删除标示为{Id}的类型统计数据成功");
                return new BaseResponse { Success = true, Message = "删除类型统计数据成功" };
            }
            catch (Exception ex)
            {
                _log.LogError($"{account}删除标示为{Id}的类型统计数据失败,失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "删除类型统计数据失败，请联系管理员" };
            }
        }

        public async Task<BaseResponse> GetStatisticsAsync(int Id)
        {
            //var data = await _tsr.FindAsync(Id);
            var data = await _tsr.FindWithFormat(a => a.Id == Id).FirstOrDefaultAsync();
            if (data == null)
            {
                return new BaseResponse { Success = false, Message = "输入的类型统计编号不存在" };
            }
            var dto = _mapper.Map<TypeStatisticsData>(data);
            return new BResponse<TypeStatisticsData> { Success = true, Message = "获取数据成功", Data = dto };
        }
        public async Task<BaseResponse> FindByTypeAsync(int typeId, TypeStatisticsPageRequestViewModel req)
        {
            //var query = _tsr.Find(a => a.TypeId == typeId);
            var query = _tsr.FindWithFormat(a => a.TypeId == typeId);
            if (!string.IsNullOrWhiteSpace(req.Search))
            {
                query = query.Where(a => a.Name.Contains(req.Search) || a.DataKey.Contains(req.Search));
            }
            int Count = query.Count();
            string OrderExpression = "";
            if (string.IsNullOrEmpty(req.OrderBy))
            {
                OrderExpression = "Id Asc";
            }
            else
            {
                OrderExpression = string.Format("{0} {1}", req.OrderBy, req.OrderType);
            }
            var data = await query.OrderBy(OrderExpression).Skip((req.PageNo - 1) * req.PageSize).Take(req.PageSize).ToListAsync();
            var dtos = _mapper.Map<List<TypeStatisticsData>>(data);
            return new BasePageResponse<List<TypeStatisticsData>>
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

        /// <summary>
        /// 获取所有首页级的统计项目
        /// </summary>
        /// <returns></returns>
        public async Task<BaseResponse> GetGlobalStatisticsAsync()
        {
            var data = await _tsr.FindGlobalStaticsBySql(1);
            var dtos = _mapper.Map<List<TypeStatisticsData>>(data);
            return new BResponse<List<TypeStatisticsData>>
            {
                Success = true,
                Message = "获取数据成功",
                Data = dtos
            };
        }
    }
}
