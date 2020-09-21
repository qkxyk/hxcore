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
    public class TypeDisplayIconService : ITypeDisplayIconService
    {
        private readonly ILogger<TypeDisplayIconService> _log;
        private readonly IMapper _map;
        private readonly ITypeDisplayIconRepository _tdr;

        public TypeDisplayIconService(ILogger<TypeDisplayIconService> log, IMapper map, ITypeDisplayIconRepository tdr)
        {
            this._log = log;
            this._map = map;
            this._tdr = tdr;
        }

        public Task<bool> IsExist(Expression<Func<TypeDisplayIconModel, bool>> predicate)
        {
            throw new NotImplementedException();
        }
        public async Task<BaseResponse> AddTypeDisplayIconAsync(string Account, int TypeId, TypeDisplayIconAddDto req)
        {
            var td = await _tdr.Find(a => a.TypeId == TypeId && a.Name == req.Name).FirstOrDefaultAsync();
            if (td != null)
            {
                return new BaseResponse { Success = false, Message = "该类型下已添加相同名称的数据" };
            }
            try
            {
                var entity = _map.Map<TypeDisplayIconModel>(req);
                entity.Create = Account;
                entity.TypeId = TypeId;
                await _tdr.AddAsync(entity);
                _log.LogInformation($"{Account}添加标示的类型显示数据成功");
                return new HandleResponse<int> { Success = true, Message = "添加数据成功", Key = entity.Id };
            }
            catch (Exception ex)
            {
                _log.LogError($"{Account}添加类型显示数据失败，失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "添加数据失败，请联系管理员" };
            }
        }
        public async Task<BaseResponse> UpdateTypeDisplayIconAsync(string Account, int TypeId, TypeDisplayIconUpdateDto req)
        {
            var td = await _tdr.FindAsync(req.Id);
            if (td == null)
            {
                return new BaseResponse { Success = false, Message = "输入的数据不存在" };
            }
            var count = await _tdr.Find(a => a.Name == req.Name && a.TypeId == TypeId && a.Id != req.Id).CountAsync();
            if (count > 0)
            {
                return new BaseResponse { Success = false, Message = "已存在相同名称的数据" };
            }
            try
            {
                var entity = _map.Map(req, td);
                entity.Modify = Account;
                entity.ModifyTime = DateTime.Now;
                await _tdr.SaveAsync(entity);
                _log.LogInformation($"{Account}修改标示为{req.Id}的类型显示数据成功");
                return new BaseResponse { Success = true, Message = "修改数据成功" };
            }
            catch (Exception ex)
            {
                _log.LogError($"{Account}修改标示为{req.Id}的类型显示数据失败，失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "修改数据失败，请联系管理员" };
            }
        }
        public async Task<BaseResponse> DeleteDisplayIconAsync(string Account, int Id)
        {
            var td = await _tdr.FindAsync(Id);
            if (td == null)
            {
                return new BaseResponse { Success = false, Message = "输入的数据不存在" };
            }
            try
            {
                await _tdr.RemoveAsync(td);
                _log.LogInformation($"{Account}删除标示为{Id}的类型显示数据成功");
                return new BaseResponse { Success = true, Message = "删除数据成功" };
            }
            catch (Exception ex)
            {
                _log.LogError($"{Account}删除标示为{Id}的类型显示数据失败，失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "删除数据失败，请联系管理员" };
            }
        }
        public async Task<BaseResponse> GetTypeDisplayByTypeIdAsync(int TypeId)
        {
            var data = await _tdr.FindWithDataDefine(a => a.TypeId == TypeId);
            var dtos = _map.Map<List<TypeDisplayIconDto>>(data);
            return new BResponse<List<TypeDisplayIconDto>> { Success = true, Message = "获取数据成功", Data = dtos };
        }
    }
}
