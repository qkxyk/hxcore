using AutoMapper;
using HXCloud.Model;
using HXCloud.Repository;
using HXCloud.ViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HXCloud.Service
{
    public class OpsItemService : IOpsItemService
    {
        private readonly ILogger<OpsItemService> _logger;
        private readonly IOpsItemRepository _opsItem;
        private readonly IMapper _mapper;
        private readonly ITypeOpsItemRepository _typeOpsItem;

        public OpsItemService(ILogger<OpsItemService> logger, IOpsItemRepository opsItem, IMapper mapper, ITypeOpsItemRepository typeOpsItem)
        {
            this._logger = logger;
            this._opsItem = opsItem;
            this._mapper = mapper;
            this._typeOpsItem = typeOpsItem;
        }
        public Task<bool> IsExist(Expression<Func<OpsItemModel, bool>> predicate)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 添加巡检项目
        /// </summary>
        /// <param name="account">操作人</param>
        /// <param name="req">巡检项目</param>
        /// <returns></returns>
        public async Task<BaseResponse> AddOpsItemAsync(string account, OpsItemAddDto req)
        {
            //检测是否添加过相同的key值
            var data = await _opsItem.Find(a => a.Key == req.Key).FirstOrDefaultAsync();
            if (data != null)
            {
                return new BaseResponse { Success = false, Message = "已添加过相同key值的数据" };
            }
            //检测类型巡检项目中是否已添加过相同的key
            var typeOpsItem = await _typeOpsItem.Find(a => a.Key == req.Key).FirstOrDefaultAsync();
            if (typeOpsItem != null)
            {
                return new HandleIdResponse<int> { Success = false, Message = $"类型标识为{typeOpsItem.TypeId}已存在相同的key值", Id = typeOpsItem.Id };
            }
            try
            {
                var entity = _mapper.Map<OpsItemModel>(req);
                entity.Create = account;
                entity.CreateTime = DateTime.Now;
                await _opsItem.AddAsync(entity);
                _logger.LogInformation($"{account}添加标示为{entity.Id}巡检项目成功");
                return new HandleResponse<int> { Success = true, Message = "添加巡检项目成功", Key = entity.Id };
            }
            catch (Exception ex)
            {
                _logger.LogError($"{account}添加Key为：{req.Key}的巡检项目失败，失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "添加巡检项目失败" };
            }
        }
        /// <summary>
        /// 更新巡检项目信息，key值不能更改
        /// </summary>
        /// <param name="account">操作人</param>
        /// <param name="req">巡检项目信息</param>
        /// <returns></returns>
        public async Task<BaseResponse> UpdateOpsItemAsync(string account, OpsItemUpdateDto req)
        {
            try
            {
                var data = await _opsItem.FindAsync(req.Id);
                if (data == null)
                {
                    return new BaseResponse { Success = false, Message = "输入的巡检项目不存在" };
                }
                else
                {
                    OpsItemModel entity = _mapper.Map(req, data);
                    await _opsItem.SaveAsync(entity);
                    _logger.LogInformation($"{account}修改标示为{entity.Id}巡检项目成功");
                    return new BaseResponse { Success = true, Message = "修改巡检项目成功" };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{account}修改标识为：{req.Id}的巡检项目失败，失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "修改巡检项目失败" };
            }
        }
        /// <summary>
        /// 删除巡检项目
        /// </summary>
        /// <param name="account">操作人</param>
        /// <param name="Id">巡检项目标识</param>
        /// <returns></returns>
        public async Task<BaseResponse> DeleteOpsItemAsync(string account, int Id)
        {
            try
            {
                var data = await _opsItem.FindAsync(Id);
                if (data == null)
                {
                    return new BaseResponse { Success = false, Message = "输入的巡检项目不存在" };
                }
                else
                {
                    await _opsItem.RemoveAsync(data);
                    _logger.LogInformation($"{account}删除标示为{Id}巡检项目成功");
                    return new BaseResponse { Success = true, Message = "删除巡检项目成功" };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{account}删除标识为：{Id}的巡检项目失败，失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "删除巡检项目失败" };
            }
        }
        /// <summary>
        /// 获取全部巡检项目
        /// </summary>
        /// <param name="req">查询条件</param>
        /// <returns></returns>
        public async Task<BaseResponse> GetOpsItemAsync(BaseRequest req)
        {
            var data = _opsItem.Find(a => true);
            if (!string.IsNullOrWhiteSpace(req.Search))
            {
                data = data.Where(a => a.Key.Contains(req.Search) || a.Name.Contains(req.Search));
            }
            string OrderExpression = "";
            if (string.IsNullOrEmpty(req.OrderBy))
            {
                OrderExpression = "Id Asc";
            }
            else
            {
                OrderExpression = string.Format("{0} {1}", req.OrderBy, req.OrderType);
            }
            var list = await data.OrderBy(OrderExpression).ToListAsync();
            var dtos = _mapper.Map<List<OpsItemDto>>(list);
            return new BResponse<List<OpsItemDto>> { Success = true, Message = "获取数据成功", Data = dtos };
        }
        /// <summary>
        /// 获取分页巡检项目
        /// </summary>
        /// <param name="req">查询条件</param>
        /// <returns></returns>
        public async Task<BaseResponse> GetOpsItemPageAsync(BasePageRequest req)
        {
            var data = _opsItem.Find(a => true);
            if (!string.IsNullOrWhiteSpace(req.Search))
            {
                data = data.Where(a => a.Key.Contains(req.Search) || a.Name.Contains(req.Search));
            }
            int count = data.Count();
            string OrderExpression = "";
            if (string.IsNullOrEmpty(req.OrderBy))
            {
                OrderExpression = "Id Asc";
            }
            else
            {
                OrderExpression = string.Format("{0} {1}", req.OrderBy, req.OrderType);
            }
            var list = await data.OrderBy(OrderExpression).Skip((req.PageNo - 1) * req.PageSize).Take(req.PageSize).ToListAsync();
            var dtos = _mapper.Map<List<OpsItemDto>>(list);
            var ret = new BasePageResponse<List<OpsItemDto>>();
            ret.Success = true;
            ret.Message = "获取数据成功";
            ret.PageSize = req.PageSize;
            ret.CurrentPage = req.PageNo;
            ret.Count = count;
            ret.TotalPage = (int)Math.Ceiling((decimal)count / req.PageSize);
            ret.Data = dtos;
            return ret;
        }
    }
}
