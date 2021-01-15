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
    /// <summary>
    /// 类型分组服务类
    /// </summary>
    public class TypeClassService : ITypeClassService
    {
        private readonly ILogger<TypeClassService> _log;
        private readonly ITypeClassRepository _tcr;
        private readonly IMapper _mapper;
        private readonly ITypeRepository _tr;

        public TypeClassService(ILogger<TypeClassService> log, ITypeClassRepository tcr, IMapper mapper, ITypeRepository tr)
        {
            this._log = log;
            this._tcr = tcr;
            this._mapper = mapper;
            this._tr = tr;
        }

        public Task<bool> IsExist(Expression<Func<TypeClassModel, bool>> predicate)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 添加类型分组数据
        /// </summary>
        /// <param name="Account">操作人</param>
        /// <param name="TypeId">类型编号</param>
        /// <param name="req">类型分组数据</param>
        /// <returns></returns>
        public async Task<BaseResponse> AddTypeClassAsync(string Account, int TypeId, TypeClassAddDto req)
        {
            //验证类型是否可以添加
            var t = await _tr.FindAsync(TypeId);
            if (t.Status == TypeStatus.Root)
            {
                return new BaseResponse { Success = false, Message = "目录节点类型不能添加具体数据" };
            }
            //验证是否重名
            var data = await _tcr.Find(a => a.TypeId == TypeId && a.Name == req.Name).FirstOrDefaultAsync();
            if (data != null)
            {
                return new BaseResponse { Success = false, Message = "已存在相同名称的类型分组，请确认" };
            }
            try
            {
                var entity = _mapper.Map<TypeClassModel>(req);
                entity.Create = Account;
                entity.TypeId = TypeId;
                await _tcr.AddAsync(entity);
                _log.LogInformation($"{Account}添加标识为{entity.Id}的类型分组数据成功");
                return new HandleResponse<int> { Key = entity.Id, Success = true, Message = "添加数据成功" };
            }
            catch (Exception ex)
            {
                _log.LogError($"{Account}添加类型分组失败,失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "添加类型分组数据失败,请联系管理员" };
            }
        }
        /// <summary>
        /// 更新类型分组数据
        /// </summary>
        /// <param name="req">类型分组信息</param>
        /// <param name="account">操作人</param>
        /// <returns></returns>
        public async Task<BaseResponse> UpdateAsync(TypeClassUpdateDto req, string account)
        {
            var data = await _tcr.FindAsync(req.Id);
            if (data == null)
            {
                return new BaseResponse { Success = false, Message = "输入的类型分组数据不存在" };
            }
            var ret = await _tcr.Find(a => a.Name == req.Name && a.TypeId == data.TypeId).FirstOrDefaultAsync();
            if (ret != null && ret.Id != req.Id)
            {
                return new BaseResponse { Success = false, Message = "已存在相同名称的类型分组" };
            }
            try
            {
                var entity = _mapper.Map(req, data);
                entity.Modify = account;
                entity.ModifyTime = DateTime.Now;
                await _tcr.SaveAsync(entity);
                _log.LogInformation($"{account}修改标示为{req.Id}的类型分组数据成功");
                return new BaseResponse { Success = true, Message = "修改类型分组数据成功" };
            }
            catch (Exception ex)
            {
                _log.LogError($"{account}修改标示为{req.Id}的类型分组失败,失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "修改类型分组数据失败" };
            }
        }
        /// <summary>
        /// 删除类型分组数据
        /// </summary>
        /// <param name="Id">类型分组标识</param>
        /// <param name="account">操作人</param>
        /// <returns></returns>
        public async Task<BaseResponse> DeleteAsync(int Id, string account)
        {
            var data = await _tcr.FindAsync(Id);
            if (data == null)
            {
                return new BaseResponse { Success = false, Message = "输入的类型分组标示不存在" };
            }
            try
            {
                await _tcr.RemoveAsync(data);
                _log.LogInformation($"{account}删除标示为{Id}的类型分组数据成功");
                return new BaseResponse { Success = true, Message = "删除类型分组数据成功" };
            }
            catch (Exception ex)
            {
                _log.LogError($"{account}删除标示为{Id}的类型分组数据失败,失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = $"编号为{Id}的类型分组正在使用中，请先删除关联的模块控制数据！" };
            }
        }

        /// <summary>
        /// 获取类型分组信息
        /// </summary>
        /// <param name="TypeId">类型标识</param>
        /// <returns></returns>
        public async Task<BaseResponse> GetByTypeIdAsync(int TypeId)
        {
            var data = await _tcr.Find(a => a.TypeId == TypeId).ToListAsync();
            var dtos = _mapper.Map<List<TypeClassDto>>(data);
            return new BResponse<List<TypeClassDto>> { Success = true, Message = "获取数据成功", Data = dtos };
        }
    }
}
