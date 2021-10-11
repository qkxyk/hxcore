using AutoMapper;
using HXCloud.Model;
using HXCloud.Repository;
using HXCloud.ViewModel;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace HXCloud.Service
{
    public class WarnCodeService : IWarnCodeService
    {
        private readonly ILogger _log;
        private readonly IMapper _mapper;
        private readonly IWarnCodeRepository _warnCode;

        public WarnCodeService(ILogger<WarnCodeService> log, IMapper mapper, IWarnCodeRepository warnCode)
        {
            this._log = log;
            this._mapper = mapper;
            this._warnCode = warnCode;
        }
        public async Task<bool> IsExist(Expression<Func<WarnCodeModel, bool>> predicate)
        {
            var data = await _warnCode.Find(predicate).FirstOrDefaultAsync();
            if (data == null)
            {
                return false;
            }
            return true;
        }

        public async Task<BaseResponse> AddWarnCodeAsync(string account, int warnTypeId, WarnCodeAddDto req)
        {
            var code = await _warnCode.FindAsync(req.Code);
            if (code != null)
            {
                return new BaseResponse { Success = false, Message = "已存在相同的代码" };
            }
            try
            {
                var entity = _mapper.Map<WarnCodeModel>(req);
                entity.WarnTypeId = warnTypeId;
                entity.Create = account;
                await _warnCode.AddAsync(entity);
                _log.LogInformation($"{account}添加代码为{req.Code}的报警代码成功");
                return new HandleResponse<string> { Success = true, Message = "添加数据成功", Key = req.Code };
            }
            catch (Exception ex)
            {
                _log.LogError($"{account}添加代码为{req.Code}的报警代码失败，失败原因：{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "添加报警代码失败，请联系管理员" };
            }
        }
        public async Task<BaseResponse> UpdateWarnCodeDescriptionAsync(string account, string code, string description)
        {
            var warnCode = await _warnCode.FindAsync(code);
            if (warnCode == null)
            {
                return new BaseResponse { Success = false, Message = "输入的报警代码不存在" };
            }
            try
            {
                warnCode.Description = description;
                warnCode.Modify = account;
                warnCode.ModifyTime = DateTime.Now;
                await _warnCode.SaveAsync(warnCode);
                _log.LogInformation($"{account}修改代码为{code}的报警代码信息成功");
                return new BaseResponse { Success = true, Message = "修改数据成功" };
            }
            catch (Exception ex)
            {
                _log.LogError($"{account}修改代码为{code}的报警代码失败，失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "修改报警代码信息失败，请联系管理员" };
            }
        }
        public async Task<BaseResponse> DeleteWarnCodeAsync(string account, string code)
        {
            var warnCode = await _warnCode.FindAsync(code);
            if (warnCode == null)
            {
                return new BaseResponse { Success = false, Message = "输入的报警代码不存在" };
            }
            try
            {
                await _warnCode.RemoveAsync(warnCode);
                _log.LogInformation($"{account}删除报警代码{code}成功");
                return new BaseResponse { Success = true, Message = "删除报警代码信息成功" };
            }
            catch (Exception ex)
            {
                _log.LogError($"{account}删除报警代码{code}失败，失败原因：{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "删除报警代码失败,请联系管理员" };
            }
        }
        public async Task<BaseResponse> GetPageWarnCodeAsync(WarnCodePageRequest req)
        {
            Expression<Func<WarnCodeModel, bool>> predicate;
            if (req.warnTypeId == 0)
            {
                predicate = a => true;
            }
            else
            {
                predicate = a => a.WarnTypeId == req.warnTypeId;
            }
            var wc = _warnCode.FindWithWarnType(predicate);
            if (!string.IsNullOrWhiteSpace(req.Search))
            {
                wc = wc.Where(a => a.Code.Contains(req.Search));
            }
            int count = wc.Count();
            string OrderExpression = "";
            if (string.IsNullOrEmpty(req.OrderBy))
            {
                OrderExpression = "Code Asc";
            }
            else
            {
                OrderExpression = string.Format("{0} {1}", req.OrderBy, req.OrderType);
            }
            var data = await wc.OrderBy(OrderExpression).Skip((req.PageNo - 1) * req.PageSize).Take(req.PageSize).ToListAsync();
            var dto = _mapper.Map<IEnumerable<WarnCodeDto>>(data);
            BaseResponse br = new BasePageResponse<IEnumerable<WarnCodeDto>>()
            {
                Count = count,
                CurrentPage = req.PageNo,
                PageSize = req.PageSize,
                Success = true,
                Message = "获取数据成功",
                Data = dto,
                TotalPage = (int)Math.Ceiling((decimal)count / req.PageSize)
            };
            return br;
        }

    }
}
