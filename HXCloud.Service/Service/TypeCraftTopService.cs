using AutoMapper;
using HXCloud.Model;
using HXCloud.Repository;
using HXCloud.ViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HXCloud.Service
{
    public class TypeCraftTopService : ITypeCraftTopService
    {
        private readonly ITypeCraftTopRepository _craftTopRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<TypeCraftTopService> _log;

        public TypeCraftTopService(ITypeCraftTopRepository craftTopRepository, IMapper mapper, ILogger<TypeCraftTopService> log)
        {
            this._craftTopRepository = craftTopRepository;
            this._mapper = mapper;
            this._log = log;
        }
        public Task<bool> IsExist(Expression<Func<TypeCraftTopModel, bool>> predicate)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 查询拓扑数据是否存在
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public async Task<TypeCraftTopExistDto> IsCraftTopExist(Expression<Func<TypeCraftTopModel, bool>> predicate)
        {
            var data = await _craftTopRepository.Find(predicate).FirstOrDefaultAsync();
            if (data == null)
            {
                return new TypeCraftTopExistDto { IsExist = false };
            }
            else
            {
                return new TypeCraftTopExistDto { IsExist = true, Url = data.Url, Account = data.Create };
            }
        }

        /// <summary>
        /// 添加类型拓扑数据
        /// </summary>
        /// <param name="account">操作人</param>
        /// <param name="req">网络拓扑数据</param>
        /// <returns></returns>
        public async Task<BaseResponse> AddTypeCraftTopAsync(string account, TypeCraftTopAddDto req)
        {
            var exist = await _craftTopRepository.Find(a => a.TypeId == req.TypeId && a.Key == req.Key).FirstOrDefaultAsync();
            if (exist != null)
            {
                return new BaseResponse { Success = false, Message = "该类型下已存在相同Key值" };
            }
            try
            {
                var entity = _mapper.Map<TypeCraftTopModel>(req);
                entity.Create = account;
                await _craftTopRepository.AddAsync(entity);
                _log.LogInformation($"{account}添加标识为{entity.Id}的类型拓扑数据成功");
                return new HandleIdResponse<int> { Id = entity.Id, Success = true, Message = "添加数据成功" };
            }
            catch (Exception ex)
            {
                _log.LogError($"{account}添加拓扑数据失败，失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "添加数据失败，请联系管理员" };
            }
        }
        /// <summary>
        /// 根据拓扑数据标识获取拓扑数据
        /// </summary>
        /// <param name="Id">拓扑数据标识</param>
        /// <returns></returns>
        public async Task<BaseResponse> GetTypeCraftTopByIdAsync(int Id)
        {
            var data = await _craftTopRepository.FindAsync(Id);
            if (data == null)
            {
                return new BaseResponse { Success = false, Message = "输入的数据不存在" };
            }
            var dto = _mapper.Map<TypeCraftTopDto>(data);
            return new BResponse<TypeCraftTopDto> { Success = true, Message = "获取数据成功", Data = dto };
        }

        /// <summary>
        /// 获取类型拓扑数据
        /// </summary>
        /// <param name="typeId">类型标识</param>
        /// <returns></returns>
        public async Task<BaseResponse> GetTypeCraftTopAsync(int typeId)
        {
            var data = await _craftTopRepository.Find(a => a.TypeId == typeId).ToListAsync();
            var dtos = _mapper.Map<List<TypeCraftTopDto>>(data);
            return new BResponse<List<TypeCraftTopDto>> { Success = true, Message = "获取数据成功", Data = dtos };
        }

        /// <summary>
        /// 删除类型拓扑数据
        /// </summary>
        /// <param name="id">数据标识</param>
        /// <param name="account">操作人</param>
        /// <param name="isAdmin">是否管理员</param>
        /// <param name="path">文件的路径</param>
        /// <returns></returns>
        public async Task<BaseResponse> DeleteTypeCraftTopAsync(int id, string account, bool isAdmin, string path)
        {
            var data = await _craftTopRepository.FindAsync(id);
            if (data == null)
            {
                return new BaseResponse { Success = false, Message = "输入的数据不存在" };
            }
            if (!isAdmin)
            {
                if (data.Create != account)
                {
                    return new BaseResponse { Success = false, Message = "用户没有权限删除该数据" };
                }
            }
            try
            {
                //先删除文件
                string url = Path.Combine(path, data.Url);
                if (System.IO.File.Exists(url))
                {
                    System.IO.File.Delete(url);
                }
                await _craftTopRepository.RemoveAsync(data);
                _log.LogInformation($"{account}删除标识为{id}的类型拓扑数据成功");
                return new BaseResponse { Success = true, Message = "删除数据成功" };
            }
            catch (Exception ex)
            {
                _log.LogError($"{account}删除标识为{id}的类型拓扑数据失败，失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "删除数据失败，请联系管理员" };
            }
        }

        /// <summary>
        /// 修改类型拓扑数据
        /// </summary>
        /// <param name="account">修改人</param>
        /// <param name="req">拓扑数据信息</param>
        /// <returns></returns>
        public async Task<BaseResponse> UpdateTypeCraftTopAsync(string account, TypeCraftTopEditDto req)
        {
            var data = await _craftTopRepository.FindAsync(req.Id);
            //检测要修改的key是否重复
            var keyData = await _craftTopRepository.Find(a => a.Id != req.Id && a.TypeId == data.TypeId && a.Key == req.Key).FirstOrDefaultAsync();
            if (keyData != null)
            {
                return new BaseResponse { Success = false, Message = "同一个类型的key不能重复" };
            }
            try
            {
                var entity = _mapper.Map(req, data);
                entity.Modify = account;
                entity.ModifyTime = DateTime.Now;
                await _craftTopRepository.SaveAsync(entity);
                _log.LogInformation($"{account}修改标识为{req.Id}的类型拓扑数据成功");
                return new BaseResponse { Success = true, Message = "修改数据成功" };
            }
            catch (Exception ex)
            {
                _log.LogError($"{account}修改标识为{req.Id}的类型拓扑数据失败，失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "修改数据失败，请联系管理员" };
            }
        }
    }
}
