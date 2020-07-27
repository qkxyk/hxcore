using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using HXCloud.Model;
using HXCloud.Repository;
using HXCloud.ViewModel;
using HXCloud.Common;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Microsoft.Extensions.Logging;
using System.Linq.Dynamic.Core;
using AutoMapper;

namespace HXCloud.Service
{
    public class GroupService : IGroupService
    {
        private readonly IMapper _mapper;
        private readonly ILogger<GroupService> _log;

        IGroupRepository _group { get; }
        public GroupService(IGroupRepository group, IMapper mapper, ILogger<GroupService> log)
        {
            _group = group;
            this._mapper = mapper;
            this._log = log;
        }
        public async Task<BaseResponse> AddGroupAsync(GroupAddViewModel req, string account)
        {
            //创建一个租户，连带创建该组织的一个账号，一个管理员角色
            BaseResponse rm = new BaseResponse();
            try
            {
                var gm = _mapper.Map<GroupModel>(req);
                gm.Id = EncryptData.CreateUUID();
                gm.Create = account;
                string salt = EncryptData.CreateRandom();
                string password = EncryptData.EncryptPassword(req.Password, salt);
                UserModel um = new UserModel()
                {
                    Account = req.Account,
                    Password = password,
                    Salt = salt,
                    CreateTime = DateTime.Now,
                    GroupId = gm.Id,
                    //IsAdmin = true,
                    Status = Model.UserStatus.Valid,
                    Create = account
                };
                await _group.Add(gm, um);
                rm.Success = true;
                rm.Message = "添加组织成功";
                _log.LogInformation($"{account}添加组织{gm.GroupName}成功");
            }
            catch (Exception ex)
            {
                rm.Success = false;
                rm.Message = "添加组织失败,请联系管理员";// + ex.Message;
                _log.LogError($"{account}添加组织{req.Name}失败，失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
            }
            return rm;
        }

        public async Task<BaseResponse> GetGroupAsync(string Id)
        {
            var gm = await _group.Find(a => a.Id == Id).FirstOrDefaultAsync();
            if (gm != null)
            {
                var br = new BResponse<GroupData>();
                br.Success = true;
                br.Message = "获取数据成功";

                br.Data = _mapper.Map<GroupData>(gm);
                return br;
            }
            else
            {
                return new BaseResponse() { Success = false, Message = "该组织数据不存在" };
            }
        }

        public async Task<bool> IsExist(Expression<Func<GroupModel, bool>> predicate)
        {
            var ret = await _group.Find(predicate).FirstOrDefaultAsync();
            if (ret == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public async Task<BaseResponse> GetGroupsAsync(GroupListRequest req)
        {
            GroupListViewModel rm = new GroupListViewModel();
            //根据要查找的字段进行查找
            Expression<Func<GroupModel, bool>> predicate;
            if (string.IsNullOrWhiteSpace(req.Field) || string.IsNullOrWhiteSpace(req.FieldValue))
            {
                predicate = a => true;
            }
            else
            {
                predicate = LinqHelper<GroupModel>.Contains(req.Field, req.FieldValue);
            }
            var groups = _group.Find(predicate);
            if (req.OrderType.ToUpper() == "ASC")
            {
                groups = groups.OrderBy(LinqHelper<GroupModel>.Order<string>(req.OrderField/*a => a.GetType().GetProperty(req.OrderField)*/));
            }
            else
            {
                groups = groups.OrderByDescending(LinqHelper<GroupModel>.Order<string>(req.OrderField));
            }
            var gml = await groups.ToListAsync();
            foreach (var item in gml)
            {
                //rm.Data.Add(new GroupData() { Id = item.Id, Code = item.Code, Description = item.Description, Logo = item.Logo, Name = item.Name });
            }
            rm.Success = true;
            rm.Message = "获取数据成功";
            return rm;
        }

        public async Task<BaseResponse> UpdateLogoAsync(string groupId, string url, string account)
        {
            BaseResponse rm = new BaseResponse();
            var g = _group.Find(groupId);
            if (g == null)
            {
                rm.Success = false;
                rm.Message = "组织不存在";
                return rm;
            }
            try
            {
                ////删除已存在的logo
                //if (System.IO.File.Exists(path + g.Logo))
                //{
                //    System.IO.File.Delete(path + g.Logo);
                //}
                g.Logo = url;
                g.Modify = account;
                g.ModifyTime = DateTime.Now;
                await _group.SaveAsync(g);
                rm.Success = true;
                rm.Message = "修改数据成功";
                _log.LogInformation($"{account}修改了组织logo成功");
                //rm.Key = groupId;
            }
            catch (Exception ex)
            {
                rm.Success = false;
                rm.Message = "修改数据失败" + ex.Message;
                _log.LogError($"{account}修改logo失败,失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
            }
            return rm;
        }
        Expression<Func<GroupModel, bool>> Contains(string propName, string propValue)
        {
            // 创建节点参数t
            ParameterExpression parameter = Expression.Parameter(typeof(GroupModel), "t");
            // 创建一个成员(字段/属性)
            MemberExpression member = Expression.PropertyOrField(parameter, propName);
            // 创建一个常数
            ConstantExpression constant = Expression.Constant(propValue, typeof(string));
            // 创建一个方法
            MethodInfo method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
            // 创建一个带参数方法Expression
            MethodCallExpression methodCall = Expression.Call(member, method, constant);
            // 生成lambda表达式
            return Expression.Lambda<Func<GroupModel, bool>>(methodCall, parameter);
        }

        public async Task<BaseResponse> GetPageGroupsAsync(GroupPageListRequest req)
        {
            BasePageResponse<List<GroupViewModel>> br = new BasePageResponse<List<GroupViewModel>>();
            var g = _group.Find(a => true);
            if (!string.IsNullOrWhiteSpace(req.Search))
            {
                g = g.Where(a => a.GroupName.Contains(req.Search) || a.GroupCode.Contains(req.Search) || a.Description.Contains(req.Search));
            }
            int count = g.Count();
            string OrderExpression = "";
            if (string.IsNullOrEmpty(req.OrderBy))
            {
                OrderExpression = "Id Asc";
                //UserQuery = UserQuery.OrderBy(a => a.Id);
            }
            else
            {
                var orderExpression = string.Format("{0} {1}", req.OrderBy, req.OrderType);
            }
            var GroupList = await g.OrderBy(OrderExpression).Skip((req.PageNo - 1) * req.PageSize).Take(req.PageSize).ToListAsync();
            var dto = _mapper.Map<List<GroupViewModel>>(GroupList);
            br.Success = true;
            br.Message = "获取数据成功";
            br.PageSize = req.PageSize;
            br.CurrentPage = req.PageNo;
            br.Count = count;
            br.TotalPage = (int)Math.Ceiling((decimal)count / req.PageSize);
            br.Data = dto;
            return br;
        }

        public async Task<BaseResponse> UpdateAsync(GroupUpdateViewModel req, string account)
        {
            //检查要修改的组织是否重名
            var d = await _group.Find(a => a.GroupName == req.GroupName && a.Id != req.GroupId).ToListAsync();
            if (d.Count() > 0)
            {
                return new BaseResponse { Success = false, Message = "已存在相同的组织名称" };
            }
            var g = _group.Find(req.GroupId);
            if (g == null)
            {
                return new BaseResponse { Success = false, Message = "该组织不存在" };
            }
            try
            {
                g.Modify = account;
                g.ModifyTime = DateTime.Now;
                g.GroupName = req.GroupName;
                g.Description = req.Description;
                await _group.SaveAsync(g);
                _log.LogInformation($"{account}修改组织{req.GroupId}:{req.GroupName}成功");
                return new BaseResponse { Success = true, Message = "修改数据成功" };
            }
            catch (Exception ex)
            {
                _log.LogError($"{account}修改组织信息失败,失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "修改数据失败，请联系管理员" };
            }
        }

        public async Task<string> GetMasterIdAsync(string code)
        {
            var ret = await _group.Find(a => a.GroupCode == code).FirstOrDefaultAsync();
            if (ret == null)
            {
                return null;
            }
            return ret.Id;
        }
    }

}
