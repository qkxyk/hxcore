using AutoMapper;
using HXCloud.Model;
using HXCloud.Repository;
using HXCloud.ViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace HXCloud.Service
{
    public class TypeOpsItemService : ITypeOpsItemService
    {
        private readonly ILogger<TypeOpsItemService> _logger;
        private readonly IMapper _mapper;
        private readonly ITypeOpsItemRepository _typeOpsItem;
        private readonly IOpsItemRepository _opsItem;
        private readonly ITypeRepository _type;

        public TypeOpsItemService(ILogger<TypeOpsItemService> logger, IMapper mapper, ITypeOpsItemRepository typeOpsItem, IOpsItemRepository opsItem,ITypeRepository type)
        {
            this._logger = logger;
            this._mapper = mapper;
            this._typeOpsItem = typeOpsItem;
            this._opsItem = opsItem;
            this._type = type;
        }
        public Task<bool> IsExist(Expression<Func<TypeOpsItemModel, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 添加类型巡检项目
        /// </summary>
        /// <param name="account">操作人</param>
        /// <param name="req">巡检项目</param>
        /// <returns></returns>
        public async Task<BaseResponse> AddOpsItemAsync(string account, TypeOpsItemAddDto req)
        {
            //检查类型是否存在
            var type = await _type.FindAsync(req.TypeId);
            if (type==null)
            {
                return new BaseResponse { Success = false, Message = "输入的类型标识不存在，请确认" };
            }
            else if(type.Status== TypeStatus.Root)
            {
                return new BaseResponse { Success = false, Message = "目录节点类型不能添加具体数据" };
            }
            //检查巡检项目是否包含相同的key
            var ops = await _opsItem.Find(a => a.Key == req.Key).CountAsync();
            if (ops > 0)
            {
                return new BaseResponse { Success = false, Message = $"巡检项目中已添加{req.Key}的key" };
            }
            //检测是否添加过相同的key值
            var data = await _typeOpsItem.Find(a => a.Key == req.Key && a.TypeId == req.TypeId).FirstOrDefaultAsync();
            if (data != null)
            {
                return new BaseResponse { Success = false, Message = $"该类型已添加过{req.Key}的key" };
            }
            try
            {
                var entity = _mapper.Map<TypeOpsItemModel>(req);
                entity.Create = account;
                await _typeOpsItem.AddAsync(entity);
                _logger.LogInformation($"{account}添加标示为{entity.Id}类型巡检项目成功");
                return new HandleResponse<int> { Success = true, Message = "添加类型巡检项目成功", Key = entity.Id };
            }
            catch (Exception ex)
            {
                _logger.LogError($"{account}添加Key为：{req.Key}的类型巡检项目失败，失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "添加类型巡检项目失败" };
                throw;
            }
        }
        /// <summary>
        /// 更新巡检项目信息，key值不能更改
        /// </summary>
        /// <param name="account">操作人</param>
        /// <param name="req">巡检项目信息</param>
        /// <returns></returns>
        public async Task<BaseResponse> UpdateOpsItemAsync(string account, TypeOpsItemUpdateDto req)
        {
            try
            {
                var data = await _typeOpsItem.FindAsync(req.Id);
                if (data == null)
                {
                    return new BaseResponse { Success = false, Message = "输入的类型巡检项目不存在" };
                }
                else
                {
                    TypeOpsItemModel entity = _mapper.Map(req, data);
                    await _typeOpsItem.SaveAsync(entity);
                    _logger.LogInformation($"{account}修改标示为{entity.Id}类型巡检项目成功");
                    return new BaseResponse { Success = true, Message = "修改类型巡检项目成功" };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{account}修改标识为：{req.Id}的类型巡检项目失败，失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "修改类型巡检项目失败" };
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
                var data = await _typeOpsItem.FindAsync(Id);
                if (data == null)
                {
                    return new BaseResponse { Success = false, Message = "输入的类型巡检项目不存在" };
                }
                else
                {
                    await _typeOpsItem.RemoveAsync(data);
                    _logger.LogInformation($"{account}删除标示为{Id}类型巡检项目成功");
                    return new BaseResponse { Success = true, Message = "删除类型巡检项目成功" };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{account}删除标识为：{Id}的类型巡检项目失败，失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "删除类型巡检项目失败" };
            }
        }
        /// <summary>
        /// 获取全部类型巡检项目
        /// </summary>
        /// <param name="req">查询条件</param>
        /// <returns></returns>
        public async Task<BaseResponse> GetOpsItemAsync(BaseRequest req, int typeId)
        {
            var data = _typeOpsItem.Find(a => a.TypeId == typeId);
            if (!string.IsNullOrWhiteSpace(req.Search))
            {
                data = data.Where(a => req.Search.Contains(a.Key) || req.Search.Contains(a.Name));
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
            var dtos = _mapper.Map<List<TypeOpsItemDto>>(list);
            return new BResponse<List<TypeOpsItemDto>> { Success = true, Message = "获取数据成功", Data = dtos };
        }
        /// <summary>
        /// 获取分页类型巡检项目
        /// </summary>
        /// <param name="req">查询条件</param>
        /// <returns></returns>
        public async Task<BaseResponse> GetOpsItemPageAsync(BasePageRequest req, int typeId)
        {
            var data = _typeOpsItem.Find(a => a.TypeId == typeId);
            if (!string.IsNullOrWhiteSpace(req.Search))
            {
                data = data.Where(a => req.Search.Contains(a.Key) || req.Search.Contains(a.Name));
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
            var dtos = _mapper.Map<List<TypeOpsItemDto>>(list);
            var ret = new BasePageResponse<List<TypeOpsItemDto>>();
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
