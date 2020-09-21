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
    public class TypeOverviewService : ITypeOverviewService
    {
        private readonly ILogger<TypeOverviewService> _log;
        private readonly IMapper _map;
        private readonly ITypeOverviewRepository _tor;

        public TypeOverviewService(ILogger<TypeOverviewService> log, IMapper map, ITypeOverviewRepository tor)
        {
            this._log = log;
            this._map = map;
            this._tor = tor;
        }

        public Task<bool> IsExist(Expression<Func<TypeOverviewModel, bool>> predicate)
        {
            throw new NotImplementedException();
        }
        public async Task<BaseResponse> AddTypeOverviewAsync(string Account, int TypeId, TypeOverViewAddDto req)
        {
            var tv = await _tor.Find(a => a.TypeId == TypeId && a.Name == req.Name).FirstOrDefaultAsync();
            if (tv != null)
            {
                return new BaseResponse { Success = false, Message = "已存在相同名称的数据" };
            }
            try
            {
                var entity = _map.Map<TypeOverviewModel>(req);
                entity.TypeId = TypeId;
                entity.Create = Account;
                await _tor.AddAsync(entity);
                _log.LogInformation($"{Account}添加标示为{entity.Id}的类型总揽数据成功");
                return new HandleResponse<int> { Success = true, Message = "添加数据成功", Key = entity.Id };
            }
            catch (Exception ex)
            {
                _log.LogError($"{Account}添加类型{TypeId}的类型总揽数据失败，失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "添加数据失败，请联系管理员" };
            }
        }
        public async Task<BaseResponse> UpdateTypeOverviewAsync(string Account, int TypeId, TypeOverviewUpdateDto req)
        {
            var td = await _tor.FindAsync(req.Id);
            if (td == null)
            {
                return new BaseResponse { Success = false, Message = "输入的数据不存在" };
            }
            var count = await _tor.Find(a => a.Name == req.Name && a.TypeId == TypeId && a.Id != req.Id).CountAsync();
            if (count > 0)
            {
                return new BaseResponse { Success = false, Message = "已存在相同名称的数据" };
            }
            try
            {
                var entity = _map.Map(req, td);
                entity.Modify = Account;
                entity.ModifyTime = DateTime.Now;
                await _tor.SaveAsync(entity);
                _log.LogInformation($"{Account}修改标示为{req.Id}的类型总揽数据成功");
                return new BaseResponse { Success = true, Message = "修改数据成功" };
            }
            catch (Exception ex)
            {
                _log.LogError($"{Account}修改标示为{req.Id}的类型总揽数据失败，失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "修改数据失败，请联系管理员" };
            }
        }

        public async Task<BaseResponse> DeleteTypeOverviewAsync(string Account, int Id)
        {
            var td = await _tor.FindAsync(Id);
            if (td == null)
            {
                return new BaseResponse { Success = false, Message = "输入的数据不存在" };
            }
            try
            {
                await _tor.RemoveAsync(td);
                _log.LogInformation($"{Account}删除标示为{Id}的类型总揽数据成功");
                return new BaseResponse { Success = true, Message = "删除数据成功" };
            }
            catch (Exception ex)
            {
                _log.LogError($"{Account}删除标示为{Id}的类型总揽数据失败，失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "删除数据失败，请联系管理员" };
            }
        }
        public async Task<BaseResponse> GetTypeOverviewByTypeIdAsync(int TypeId)
        {
            var data = await _tor.FindWithDataDefine(a => a.TypeId == TypeId);
            var dtos = _map.Map<List<TypeOverviewDto>>(data);
            return new BResponse<List<TypeOverviewDto>> { Success = true, Message = "获取数据成功", Data = dtos };
        }
    }
}
