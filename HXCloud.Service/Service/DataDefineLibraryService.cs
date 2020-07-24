using System;
using System.Collections.Generic;
using System.Linq;
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

namespace HXCloud.Service
{
    public class DataDefineLibraryService : IDataDefineLibraryService
    {
        private readonly ILogger<DataDefineLibraryService> _log;
        private readonly IDataDefineLibraryRepository _dlr;
        private readonly IMapper _mapper;

        public DataDefineLibraryService(ILogger<DataDefineLibraryService> log, IDataDefineLibraryRepository dlr, IMapper mapper)
        {
            this._log = log;
            this._dlr = dlr;
            this._mapper = mapper;
        }
        public async Task<bool> IsExist(Expression<Func<DataDefineLibraryModel, bool>> predicate)
        {
            var ret = await _dlr.Find(predicate).FirstOrDefaultAsync();
            if (ret != null)
            {
                return true;
            }
            return false;
        }

        public async Task<BaseResponse> AddDataDefineAsync(DataDefineLibraryAddDto req, string account)
        {
            var data = await _dlr.Find(a => a.DataKey == req.DataKey || a.DataName == req.DataName).FirstOrDefaultAsync();
            if (data != null)
            {
                return new BaseResponse { Success = false, Message = "已存在相同的Key或者相同名称的数据定义" };
            }
            var entity = _mapper.Map<DataDefineLibraryModel>(req);
            try
            {
                entity.Create = account;
                await _dlr.AddAsync(entity);
                _log.LogInformation($"{account}添加数据定义库数据成功,添加的key为{req.DataKey}");
                return new HandleResponse<int>() { Success = true, Message = "添加数据成功", Key = entity.Id };
            }
            catch (Exception ex)
            {
                _log.LogError($"{account}添加key值为：{req.DataKey}数据定义库失败,失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "添加数据失败,请联系管理员" };
            }
        }

        public async Task<BaseResponse> UpdateDataDefineAsync(DataDefineLibraryUpdateDto req, string account)
        {
            var data = await _dlr.FindAsync(req.Id);
            if (data == null)
            {
                return new BaseResponse { Success = false, Message = "输入的编号不存在" };
            }
            //验证是否重名
            var ret = await _dlr.Find(a => a.DataName == req.DataName && a.Id != req.Id).FirstOrDefaultAsync();
            if (ret != null)
            {
                return new BaseResponse { Success = false, Message = "已存在相同名称的数据定义" };
            }
            var entity = _mapper.Map(req, data);
            try
            {
                entity.Modify = account;
                entity.ModifyTime = DateTime.Now;
                await _dlr.SaveAsync(data);
                _log.LogInformation($"{account}修改Id为{req.Id}数据定义库成功");
                return new BaseResponse { Success = true, Message = "修改数据成功" };
            }
            catch (Exception ex)
            {
                _log.LogError($"{account}修改Id为{req.Id}的数据定义库失败，失败原因{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "修改数据失败,请联系管理员" };
            }
        }

        public async Task<BaseResponse> DeleteDataDefineAsync(int Id, string account)
        {
            var ret = await _dlr.FindAsync(Id);
            if (ret == null)
            {
                return new BaseResponse { Success = false, Message = "输入的编号不存在" };
            }
            try
            {
                await _dlr.RemoveAsync(ret);
                _log.LogInformation($"{account}删除Id为{Id},Key值为{ret.DataKey}的数据定义库成功");
                return new BaseResponse { Success = true, Message = "删除数据成功" };
            }
            catch (Exception ex)
            {
                _log.LogError($"{account}删除Id为{Id}的数据定义库失败，失败原因{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "删除数据失败,请联系管理员" };
            }
        }

        public async Task<BaseResponse> GetDataDefineLibraryAsync(int Id)
        {
            var data = await _dlr.FindAsync(Id);
            if (data == null)
            {
                return new BaseResponse { Success = false, Message = "输入的编号不存在" };
            }
            var dto = _mapper.Map<DataDefineLibraryDataDto>(data);
            return new BResponse<DataDefineLibraryDataDto>() { Success = true, Message = "获取数据成功", Data = dto };
        }

        public async Task<BaseResponse> GetDataDefineLibrarysAsync(BasePageRequest req)
        {
            var ret = _dlr.Find(a => true);
            if (!string.IsNullOrWhiteSpace(req.Search))
            {
                ret = ret.Where(a => a.DataKey.Contains(req.Search) || a.DataName.Contains(req.Search));
            }
            int Count = ret.Count();
            string OrderExpression = "";
            if (string.IsNullOrEmpty(req.OrderBy))
            {
                OrderExpression = "Id Asc";
                //UserQuery = UserQuery.OrderBy(a => a.Id);
            }
            else
            {
                var orderExpression = string.Format("{0} {1}", req.OrderBy, req.OrderType);
            }
            var data = await ret.OrderBy(OrderExpression).Skip((req.PageNo - 1) * req.PageSize).Take(req.PageSize).ToListAsync();
            var dto = _mapper.Map<IEnumerable<DataDefineLibraryDataDto>>(data);
            BaseResponse br = new BasePageResponse<IEnumerable<DataDefineLibraryDataDto>>()
            {
                Count = Count,
                CurrentPage = req.PageNo,
                PageSize = req.PageSize,
                Success = true,
                Message = "获取数据成功",
                Data = dto,
                TotalPage = (int)Math.Ceiling((decimal)Count / req.PageSize)
            };
            return br;
        }
    }
}
