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
    public class DataDefineWarnCodeService : IDataDefineWarnCodeService
    {
        private readonly ILogger<DataDefineWarnCodeService> _log;
        private readonly IMapper _mapper;
        private readonly IDataDefineLibraryService _dls;
        private readonly IWarnCodeService _wcs;
        private readonly IDataDefineWarnCodeRepository _dcr;

        public DataDefineWarnCodeService(ILogger<DataDefineWarnCodeService> log, IMapper mapper, IDataDefineLibraryService dls, IWarnCodeService wcs, IDataDefineWarnCodeRepository dcr)
        {
            this._log = log;
            this._mapper = mapper;
            this._dls = dls;
            this._wcs = wcs;
            this._dcr = dcr;
        }
        public async Task<bool> IsExist(Expression<Func<DataDefineWarnCodeModel, bool>> predicate)
        {
            throw new NotImplementedException();
        }
        public async Task<DataDefineWarnCodeCheckDto> CheckDataDefineWarnCodeAsync(DataDefineWarnCodeAddDto req)
        {
            var code = await _wcs.IsExist(a => a.Code == req.Code);
            if (!code)
            {
                return new DataDefineWarnCodeCheckDto { IsExist = false, Message = "输入的报警编码不存在" };
            }
            var key = await _dls.IsExist(a => a.DataKey == req.DataKey);
            if (!key)
            {
                return new DataDefineWarnCodeCheckDto { IsExist = false, Message = "输入的数据定义标识不存在" };
            }
            return new DataDefineWarnCodeCheckDto { IsExist = true };
        }

        public async Task<BaseResponse> AddDataDefineWarnCodeAsync(string account, DataDefineWarnCodeAddDto req)
        {
            //检测是否已经添加过
            var ret = await _dcr.Find(a => a.DataKey == req.DataKey && a.Code == req.Code).FirstOrDefaultAsync();
            if (ret != null)
            {
                return new BaseResponse { Success = false, Message = "该数据已存在，请确认" };
            }
            try
            {
                var entity = _mapper.Map<DataDefineWarnCodeModel>(req);
                await _dcr.AddAsync(entity);
                _log.LogInformation($"{account}添加标示为{entity.Id}的数据成功");
                return new HandleResponse<int> {Key=entity.Id, Success = true, Message = "添加配置数据成功" };
            }
            catch (Exception ex)
            {
                _log.LogError($"添加数据定义关联报警数据失败，失败原因：{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "添加数据失败，请联系管理员" };
            }
        }

        public async Task<BaseResponse> RemoveDataDefineWarnCodeAsync(string account, int Id)
        {
            var entity = await _dcr.FindAsync(Id);
            if (entity == null)
            {
                return new BaseResponse { Success = false, Message = "要删除的数据不存在" };
            }
            try
            {
                await _dcr.RemoveAsync(entity);
                _log.LogInformation($"{account}删除标示为{Id}的数据定义关联报警编码数据成功");
                return new BaseResponse { Success = true, Message = "删除数据成功" };
            }
            catch (Exception ex)
            {
                _log.LogError($"{account}删除标示为{Id}的数据定义关联报警数据失败，失败原因{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "删除数据失败" };
            }
        }

        public async Task<BaseResponse> GetDataDefineWarnCodesAsync(bool flag, string[] data)
        {
            IEnumerable<DataDefineWarnCodeModel> req = null;
            if (flag)
            {
                req = await _dcr.Find(a => data.Contains(a.DataKey)).ToListAsync();
            }
            else
            {
                req = await _dcr.Find(a => data.Contains(a.Code)).ToListAsync();
            }
            var dtos = _mapper.Map<List<DataDefineWarnCodeDto>>(req);
            return new BResponse<List<DataDefineWarnCodeDto>> { Success = true, Message = "获取数据成功", Data = dtos };
        }

        public async Task<BaseResponse> GetPageDataDefineWarnCodesAsync(DataDefineWarnCodePageRequest req)
        {
            var data = _dcr.Find(a => true == true);
            int count = data.Count();
            if (!string.IsNullOrWhiteSpace(req.Search))
            {
                data = data.Where(a => a.DataKey == req.Search || a.Code == req.Search);
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
            var ret = await data.OrderBy(OrderExpression).Skip((req.PageNo - 1) * req.PageSize).Take(req.PageSize).ToListAsync();
            var dtos = _mapper.Map<List<DataDefineWarnCodeDto>>(ret);
            var br = new BasePageResponse<List<DataDefineWarnCodeDto>>();
            br.Success = true;
            br.Message = "获取数据成功";
            br.PageSize = req.PageSize;
            br.CurrentPage = req.PageNo;
            br.Count = count;
            br.TotalPage = (int)Math.Ceiling((decimal)count / req.PageSize);
            br.Data = dtos;
            return br;
        }
    }
}
