using AutoMapper;
using HXCloud.Model;
using HXCloud.Repository;
using HXCloud.ViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace HXCloud.Service
{
    public class IssueService : IIssueService
    {
        private readonly ILogger<IssueService> _log;
        private readonly IMapper _mapper;
        private readonly IIssueRepository _ir;

        public IssueService(ILogger<IssueService> log, IMapper mapper, IIssueRepository ir)
        {
            this._log = log;
            this._mapper = mapper;
            this._ir = ir;
        }
        public async Task<bool> IsExist(Expression<Func<IssueModel, bool>> predicate)
        {
            var data = await _ir.Find(predicate).FirstOrDefaultAsync();
            if (data == null)
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 查询问题单
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <returns></returns>
        public async Task<IssueModel> IsExistAsync(Expression<Func<IssueModel, bool>> predicate)
        {
            var data = await _ir.Find(predicate).FirstOrDefaultAsync();
            return data;
        }

        /// <summary>
        /// 添加上报问题
        /// </summary>
        /// <param name="account">操作人</param>
        /// <param name="req">上报数据</param>
        /// <returns>返回操作是否成功</returns>
        public async Task<BaseResponse> AddIssueAsync(string account, IssueAddDto req)
        {
            try
            {
                var entity = _mapper.Map<IssueModel>(req);
                entity.Dt = DateTime.Now;
                entity.Create = account;
                await _ir.AddAsync(entity);
                _log.LogInformation($"{account}添加标示为{entity.Id}上报数据成功");
                return new HandleResponse<int> { Success = true, Message = "添加上报数据成功", Key = entity.Id };
            }
            catch (Exception ex)
            {
                _log.LogError($"{account}添加上报数据失败，失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "添加上报数据失败" };
            }
        }
        /// <summary>
        /// 获取问题单数据
        /// </summary>
        /// <param name="Id">问题单编号</param>
        /// <returns></returns>
        public async Task<BaseResponse> GetIssueByIdAsync(int Id)
        {
            var data = await _ir.FindAsync(Id);
            if (data == null)
            {
                return null;
            }
            var dto = _mapper.Map<IssueDto>(data);
            return new BResponse<IssueDto> { Success = true, Message = "获取数据成功", Data = dto };
        }
        /// <summary>
        /// 处理问题单，已处理的问题单不能重复处理
        /// </summary>
        /// <param name="account">处理人</param>
        /// <param name="req">问题单信息</param>
        /// <returns></returns>
        public async Task<BaseResponse> UpdateIssueAsync(string account, IssueUpdateDto req)
        {
            try
            {
                var data = await _ir.FindAsync(req.Id);
                if (data == null)
                {
                    return new BaseResponse { Success = false, Message = "输入的问题单号不存在" };
                }
                if (data.Status)
                {
                    return new BaseResponse { Success = false, Message = "输入的问题单号已被处理过，不能重复处理" };
                }
                var entity = _mapper.Map(req, data);
                entity.Modify = account;
                entity.ModifyTime = DateTime.Now;
                await _ir.SaveAsync(entity);
                _log.LogInformation($"{account}修改标示为{req.Id}的问题单成功");
                return new BaseResponse { Success = true, Message = "修改数据成功" };
            }
            catch (Exception ex)
            {
                _log.LogError($"{account}处理标示为{req.Id}的问题单失败，失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "处理问题单失败，请联系管理员" };
            }
        }
        /// <summary>
        /// 删除问题单
        /// </summary>
        /// <param name="account">操作人</param>
        /// <param name="Id">问题单标识</param>
        ///<param name="path">图片保存的目录路径</param>
        /// <returns></returns>
        public async Task<BaseResponse> DeleteIssueAsync(string account, int Id, string path)
        {
            try
            {
                var data = await _ir.FindAsync(Id);
                string[] url = null;
                url = JsonConvert.DeserializeObject<string[]>(data.Url);
                //url = data.Url.Split(';');
                await _ir.RemoveAsync(data);
                //删除成功清除巡检图片
                if (url != null)
                {
                    foreach (var item in url)
                    {
                        var imagePath = Path.Combine(path, item);
                        if (File.Exists(imagePath))
                        {
                            File.Delete(imagePath);
                        }
                    }
                }
                _log.LogInformation($"{account}删除标示为{Id}的问题单成功");
                return new BaseResponse { Success = true, Message = "删除数据成功" };
            }
            catch (Exception ex)
            {
                _log.LogError($"{account}删除标示为{Id}的问题单失败，失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "删除问题单失败，请联系管理员" };
            }
        }
        /// <summary>
        ///获取用户的分页问题单, 查询三种类型，管理员权限，查询个人的，根据用户角色查询的
        /// </summary>
        /// <param name="req">查询条件</param>
        /// <param name="isAdmin">是否管理员</param>
        /// <param name="account">非管理员没有查询权限的查找自己</param>
        /// <param name="DeviceSn">非管理员有查询权限查看的设备列表</param>
        /// <returns></returns>
        public async Task<BaseResponse> GetPageIssueAsync(IssuePageRequest req, bool isAdmin, string account, List<string> DeviceSn)
        {
            var data = _ir.Find(a => a.Status == req.Status);
            if (!string.IsNullOrWhiteSpace(req.Search))
            {
                data = data.Where(a => a.DeviceName.Contains(req.Search));
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
            if (!isAdmin)
            {
                if (account != null)//查询自己上报的数据
                {
                    data = data.Where(a => a.Create == account);
                }
                else//有查询权限的角色查看有权限设备的数据
                {
                    data = data.Where(a => DeviceSn.Contains(a.DeviceSn));
                }
            }
            var list = await data.OrderBy(OrderExpression).Skip((req.PageNo - 1) * req.PageSize).Take(req.PageSize).ToListAsync();
            var dtos = _mapper.Map<List<IssueDto>>(list);
            var ret = new BasePageResponse<List<IssueDto>>();
            ret.Success = true;
            ret.Message = "获取数据成功";
            ret.PageSize = req.PageSize;
            ret.CurrentPage = req.PageNo;
            ret.Count = count;
            ret.TotalPage = (int)Math.Ceiling((decimal)count / req.PageSize);
            ret.Data = dtos;
            return ret;
        }
        [Obsolete("")]
        /// <summary>
        /// 获取用户的问题单
        /// </summary>
        /// <param name="account">用户列表</param>
        /// <param name="req">用户的请求数据，可以根据设备的名称查找</param>
        /// <returns>返回用户的问题单</returns>
        public async Task<BasePageResponse<List<IssueDto>>> GetIssuePageRequestAsync(List<string> account, IssuePageRequest req)
        {
            var data = _ir.GetIssue(account);
            data = data.Where(a => a.Status == req.Status);
            if (!string.IsNullOrWhiteSpace(req.Search))
            {
                data = data.Where(a => a.DeviceName.Contains(req.Search));
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
            var dtos = _mapper.Map<List<IssueDto>>(list);
            var ret = new BasePageResponse<List<IssueDto>>();
            ret.Success = true;
            ret.Message = "获取数据成功";
            ret.PageSize = req.PageSize;
            ret.CurrentPage = req.PageNo;
            ret.Count = count;
            ret.TotalPage = (int)Math.Ceiling((decimal)count / req.PageSize);
            ret.Data = dtos;
            return ret;
        }
        [Obsolete]
        /// <summary>
        /// 根据问题单编号获取问题单信息
        /// </summary>
        /// <param name="Id">问题单编号</param>
        /// <param name="users">用户列表</param>
        /// <returns></returns>
        public async Task<BaseResponse> GetIssueByIdAsync(int Id, List<string> users)
        {
            var data = await _ir.FindAsync(Id);
            if (data == null)
            {
                return new BaseResponse { Success = false, Message = "输入的问题单编号不存在" };
            }
            if (!users.Contains(data.Create))
            {
                return new BaseResponse { Success = false, Message = "用户没有权限查看该问题单" };
            }
            var dto = _mapper.Map<IssueDto>(data);
            return new BResponse<IssueDto> { Success = true, Message = "获取数据成功", Data = dto };
        }
    }
}
