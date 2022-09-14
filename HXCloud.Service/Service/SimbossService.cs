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
    /// <summary>
    /// simboss账号配置信息，主要是录入simboss账号，获取simboss appid和appsecret等
    /// </summary>
    public class SimbossService : ISimbossService
    {
        private readonly ILogger<SimbossService> _log;
        private readonly IMapper _mapper;
        private readonly ISimbossRepository _simboss;

        public SimbossService(ILogger<SimbossService> log, IMapper mapper, ISimbossRepository simboss)
        {
            this._log = log;
            this._mapper = mapper;
            this._simboss = simboss;
        }
        public Task<bool> IsExist(Expression<Func<SimbossModel, bool>> predicate)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 添加simboss数据
        /// </summary>
        /// <param name="Account">操作人</param>
        /// <param name="req">simboss数据</param>
        /// <returns></returns>
        public async Task<BaseResponse> AddSimbossAsync(string Account, SimbossAddDto req)
        {
            //检查是否已存在
            var count = await _simboss.Find(a => a.SimAccount == req.SimAccount).CountAsync();
            if (count > 0)
            {
                return new BaseResponse { Success = false, Message = "该账号已存在" };
            }
            try
            {
                var simboss = _mapper.Map<SimbossModel>(req);
                simboss.Create = Account;
                simboss.CreateTime = DateTime.Now;
                await _simboss.AddAsync(simboss);
                _log.LogInformation($"{Account}添加标识为{simboss.Id}的simboss数据成功");
                return new HandleResponse<int> { Key = simboss.Id, Success = true, Message = "添加数据成功" };
            }
            catch (Exception ex)
            {
                _log.LogError($"{Account}添加simboss数据失败，失败数据{req.ToString()},失败原因{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "添加数据失败，请联系管理员" };
            }
        }
        /// <summary>
        /// 更新simboss数据
        /// </summary>
        /// <param name="Account">操作人</param>
        /// <param name="req">simboss数据</param>
        /// <returns></returns>
        public async Task<BaseResponse> UpdateSimbossAsync(string Account, SimbossUpdateDto req)
        {
            //检查是否存在
            var simboss = await _simboss.FindAsync(req.Id);
            if (simboss == null)
            {
                return new BaseResponse { Success = false, Message = "输入的标识不存在,请确认" };
            }
            try
            {
                var entity = _mapper.Map(req, simboss);
                entity.Modify = Account;
                entity.ModifyTime = DateTime.Now;
                await _simboss.SaveAsync(entity);
                _log.LogInformation($"{Account}修改标识为{simboss.Id}的simboss数据成功");
                return new HandleResponse<int> { Key = req.Id, Success = true, Message = "修改数据成功" };
            }
            catch (Exception ex)
            {
                _log.LogError($"{Account}修改simboss数据失败，失败数据{req.ToString()},失败原因{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "修改数据失败，请联系管理员" };
            }
        }
        /// <summary>
        /// 删除simboss数据
        /// </summary>
        /// <param name="Account">操作人</param>
        /// <param name="Id">要删除的标识</param>
        /// <returns></returns>
        public async Task<BaseResponse> DeleteSimbossAsync(string Account, int Id)
        {
            //检查是否存在
            var simboss = await _simboss.FindAsync(Id);
            if (simboss == null)
            {
                return new BaseResponse { Success = false, Message = "输入的标识不存在,请确认" };
            }
            try
            {
                await _simboss.RemoveAsync(simboss);
                _log.LogInformation($"{Account}删除标识为{simboss.Id}的simboss数据成功");
                return new BaseResponse { Success = true, Message = "删除数据成功" };
            }
            catch (Exception ex)
            {
                _log.LogError($"{Account}删除标识为{Id}的simboss数据失败,失败原因{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "删除数据失败，请联系管理员" };
            }
        }
        /// <summary>
        /// 获取simboss数据。注：数据较少，不做分页
        /// </summary>
        /// <returns></returns>
        public async Task<BaseResponse> GetSimbossAsync()
        {
            var simbosses = await _simboss.Find(a => true).ToListAsync();
            var data = _mapper.Map<List<SimbossDto>>(simbosses);
            return new BResponse<List<SimbossDto>> { Success = true, Message = "获取数据成功", Data = data };
        }

    }
}
