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
    public class OpsFaultService : IOpsFaultService
    {
        private readonly IMapper _map;
        private readonly ILogger<OpsFaultService> _log;
        private readonly IOpsFaultRepository _opsFault;

        public OpsFaultService(IMapper map,ILogger<OpsFaultService> log,IOpsFaultRepository opsFault)
        {
            this._map = map;
            this._log = log;
            this._opsFault = opsFault;
        }
        public async Task<bool> IsExist(Expression<Func<OpsFaultModel, bool>> predicate)
        {
            var data = await _opsFault.Find(predicate).FirstOrDefaultAsync();
            if (data!=null)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 添加运维故障数据
        /// </summary>
        /// <param name="accout">操作人</param>
        /// <param name="req">故障数据</param>
        /// <returns></returns>
        public async Task<BaseResponse> AddOpsFaultAsync(string accout,OpsFaultAddDto req)
        {
            try
            {
                var entity = _map.Map<OpsFaultModel>(req);
                entity.Create = accout;
               await  _opsFault.AddAsync(entity);
                _log.LogInformation($"{accout}添加{entity.Code}的故障数据成功");
                return new HandleResponse<string> { Key = entity.Code, Success = true, Message = "添加数据成功" };
            }
            catch (Exception ex)
            {
                _log.LogError($"{accout}添加故障数据失败，失败原因:{ex.Message}->{ex.InnerException}->{ex.StackTrace}");
                return new BaseResponse { Success = false, Message = "添加故障数据失败" };
            }
        }
        /// <summary>
        /// 修改故障数据
        /// </summary>
        /// <param name="account">操作人</param>
        /// <param name="req">故障数据</param>
        /// <returns></returns>
        public async Task<BaseResponse> EditOpsFaultAsync(string account,OpsFaultEditDto req)
        {
            var entity = await _opsFault.FindAsync(req.Code);
            try
            {
                _map.Map(req, entity);
                entity.Modify = account;
                entity.ModifyTime = DateTime.Now;
                await _opsFault.SaveAsync(entity);
                _log.LogInformation($"{account}修改标识为{req.Code}的运维故障数据成功");
                return new BaseResponse { Success = true, Message = "修改数据成功" };
            }
            catch (Exception ex)
            {
                _log.LogError($"{account}修改标识为{req.Code}的运维故障数据失败，失败原因:{ex.Message}->{ex.InnerException}->{ex.StackTrace}");
                return new BaseResponse { Success = false, Message = "修改故障数据失败" };
            }
        }
        /// <summary>
        /// 删除故障数据
        /// </summary>
        /// <param name="accout">操作人</param>
        /// <param name="code">故障Code</param>
        /// <returns></returns>
        public async Task<BaseResponse> DeleteOpsFaultAsync(string accout,string code)
        {
            try
            {
                var entity = await _opsFault.FindAsync(code);
                if (entity==null)
                {
                    return new BaseResponse { Success = false, Message = "输入的故障码不存在" };
                }
                //此处需要清理已使用的运维表中的故障码
              await  _opsFault.RemoveAsync(entity);
                _log.LogInformation($"{accout}删除故障码为{code}的故障数据成功");
                return new BaseResponse { Success = true, Message = "删除数据成功" };
            }
            catch (Exception ex)
            {
                _log.LogError($"{accout}删除故障码为{code}的故障数据失败，失败原因:{ex.Message}->{ex.InnerException}->{ex.StackTrace}");
                return new BaseResponse { Success = false, Message = "删除故障数据失败" };
            }
        }
        /// <summary>
        /// 根据故障码获取故障数据
        /// </summary>
        /// <param name="code">故障码</param>
        /// <returns></returns>
        public async Task<BaseResponse> GetOpsFaultByCodeAsync(string code)
        {
            var data = await _opsFault.GetOpsFaultByCode(code);
            if (data==null)
            {
                return new BaseResponse { Success = false, Message = "输入的故障数据不存在" };
            }
            var dto = _map.Map<OpsFaultDto>(data);
            return new BResponse<OpsFaultDto> { Success = true, Message = "获取数据成功", Data = dto };
        }
    }
}
