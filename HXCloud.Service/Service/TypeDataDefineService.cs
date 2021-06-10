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
using System.Text.RegularExpressions;

namespace HXCloud.Service
{
    public class TypeDataDefineService : ITypeDataDefineService
    {
        private readonly ILogger<TypeDataDefineService> _log;
        private readonly IMapper _mapper;
        private readonly ITypeDataDefineRepository _td;
        private readonly IDataDefineLibraryRepository _ddl;
        private readonly ITypeRepository _tr;
        private readonly ICategoryRepository _cr;

        public TypeDataDefineService(ILogger<TypeDataDefineService> log, IMapper mapper, ITypeDataDefineRepository td, IDataDefineLibraryRepository ddl, ITypeRepository tr, ICategoryRepository cr)
        {
            this._log = log;
            this._mapper = mapper;
            this._td = td;
            this._ddl = ddl;
            this._tr = tr;
            this._cr = cr;
        }
        public async Task<bool> IsExist(Expression<Func<TypeDataDefineModel, bool>> predicate)
        {
            var ret = await _td.Find(predicate).FirstOrDefaultAsync();
            if (ret == null)
            {
                return false;
            }
            return true;
        }
        public async Task<string> GetTypeGroupIdAsync(int id)
        {
            var ret = await _td.GetTypeDataDefineWithTypeAsync(a => a.Id == id);
            if (ret == null || ret.Type == null)
            {
                return null;
            }
            return ret.Type.GroupId;
        }

        //调用者检查类型是否存在
        public async Task<BaseResponse> AddTypeDataDefine(int typeId, TypeDataDefineAddViewModel req, string account)
        {
            //验证类型是否可以添加
            var t = await _tr.FindAsync(typeId);
            if (t.Status == TypeStatus.Root)
            {
                return new BaseResponse { Success = false, Message = "目录节点类型不能添加具体数据" };
            }
            var dataDefine = await _ddl.FindAsync(req.DataDefineId);
            if (dataDefine == null)
            {
                return new BaseResponse { Success = false, Message = "输入的数据类型定义标示不存在" };
            }
            //检查是否已存在相同key值
            var ret = await _td.Find(a => a.TypeId == typeId && a.DataKey == dataDefine.DataKey).FirstOrDefaultAsync();
            if (ret != null)
            {
                return new BaseResponse { Success = false, Message = "该类型下存在相同key值" };
            }
            try
            {
                var entity = _mapper.Map<TypeDataDefineModel>(req);
                entity.Create = account;
                entity.TypeId = typeId;
                entity.DataKey = dataDefine.DataKey;
                entity.DataName = dataDefine.DataName;
                entity.Unit = dataDefine.Unit;
                entity.DataType = dataDefine.DataType;
                entity.DefaultValue = dataDefine.DefaultValue;
                entity.Format = dataDefine.Format;
                entity.Model = (DataDefineModel)dataDefine.Model;
                entity.Category = dataDefine.Category;
                await _td.AddAsync(entity);
                _log.LogInformation($"{account}添加类型数据定义{entity.DataKey}成功");
                return new HandleResponse<int> { Success = true, Message = "添加数据成功", Key = entity.Id };
            }
            catch (Exception ex)
            {
                _log.LogError($"{account}添加类型数据定义{dataDefine.DataKey}失败,失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "添加数据失败，请联系管理员" };
            }
        }

        //不能修改key值
        public async Task<BaseResponse> TypeDataDefineUpdate(TypeDataDefineUpdateViewModel req, string account)
        {
            var ret = await _td.FindAsync(req.Id);
            if (ret == null)
            {
                return new BaseResponse { Success = false, Message = "输入的类型数据定义不存在" };
            }
            try
            {
                var entity = _mapper.Map(req, ret);
                entity.Modify = account;
                entity.ModifyTime = DateTime.Now;

                await _td.SaveAsync(entity);
                _log.LogInformation($"{account}修改数据定义{req.Id}成功");
                return new BaseResponse { Success = true, Message = "修改数据成功" };
            }
            catch (Exception ex)
            {
                _log.LogError($"{account}修改{req.Id}的数据定义失败,失败原因：{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "修改数据失败，请联系管理员" };
            }
        }
        //调用者检查是否有权限操作
        public async Task<BaseResponse> DeleteTypeDataDefine(int Id, string account)
        {
            var ret = await _td.FindAsync(Id);
            if (ret == null)
            {
                return new BaseResponse { Success = false, Message = "输入的类型数据定义不存在" };
            }
            try
            {
                await _td.RemoveAsync(ret);
                _log.LogInformation($"{account}删除类型数据定义{Id}成功");
                return new BaseResponse { Success = true, Message = "删除数据成功" };
            }
            catch (Exception ex)
            {
                _log.LogError($"{account}删除类型数据定义失败，失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "删除类型数据定义失败，请联系管理员" };
            }
        }
        //调用者验证是否有权限
        public async Task<BaseResponse> GetDataDefine(int Id)
        {
            var data = await _td.FindAsync(Id);
            if (data == null)
            {
                return new BaseResponse { Success = false, Message = "输入的数据定义不存在" };
            }
            var dto = _mapper.Map<TypeDataDefineData>(data);
            #region 替换categroy
            if (dto.Category != null && dto.Category.Length > 0)
            {
                var cr = await _cr.Find(a => true).ToListAsync();
                var cid = dto.Category.Split(',');
                string cname = "";
                for (int i = 0; i < cid.Length; i++)
                {
                    if (cid[i].Trim() != "")
                    {
                        if (cname == "")
                        {
                            cname += cr.FirstOrDefault(a => a.Id.ToString() == cid[i]).Name;
                        }
                        else
                        {
                            cname += "," + cr.FirstOrDefault(a => a.Id.ToString() == cid[i]).Name;
                        }
                    }
                }
                dto.Category = cname;
            }
            #endregion
            return new BResponse<TypeDataDefineData> { Success = true, Message = "获取数据成功", Data = dto };
        }

        //对datakey或者dataname进行查询
        public async Task<BaseResponse> GetTypeDataDefines(int typeId, TypeDataDefinePageRequest req)
        {
            var data = _td.Find(a => a.TypeId == typeId);
            if (!string.IsNullOrWhiteSpace(req.Search))
            {
                data = data.Where(a => a.DataKey.Contains(req.Search) || a.DataName.Contains(req.Search));
            }
            //单个标签查找，第一步先查找包含的id，第二步查找id包含的，如果是多个标签的或查找需要根据标签或
            //Regex r = new Regex(@"(?=(,|\b)1\b)|(?=(,|\b)2\b)")
            if (req.CategoryId != 0)
            {
                string txt = $@"(?=(,|\b){req.CategoryId}\b)";
                Regex r = new Regex(@txt);
                var Ids = _td.Find(a => a.TypeId == typeId).ToList().Where(a =>
                {
                    if (a.Category == null)
                    {
                        return false;
                    }
                    else
                    {
                        return r.Match(a.Category).Success == true;
                    }
                }).Select(a => a.Id).ToList();
                data = data.Where(a => Ids.Contains(a.Id));
            }
            int count = data.Count();
            string OrderExpression = "";
            if (string.IsNullOrEmpty(req.OrderBy))
            {
                OrderExpression = "Id Asc";
                //UserQuery = UserQuery.OrderBy(a => a.Id);
            }
            else
            {
                OrderExpression = string.Format("{0} {1}", req.OrderBy, req.OrderType);
            }

            var entityList = await data.OrderBy(OrderExpression).Skip((req.PageNo - 1) * req.PageSize).Take(req.PageSize).ToListAsync();
            var dtos = _mapper.Map<List<TypeDataDefineData>>(entityList);
            #region 替换category
            var cr = await _cr.Find(a => true).ToListAsync();
            if (cr.Count > 0)
            {
                foreach (var item in dtos)
                {
                    if (item.Category != null && item.Category.Length > 0)
                    {
                        var cid = item.Category.Split(',');
                        string cname = "";
                        for (int i = 0; i < cid.Length; i++)
                        {
                            if (cid[i].Trim() != "")
                            {
                                if (cname == "")
                                {
                                    cname += cr.FirstOrDefault(a => a.Id.ToString() == cid[i]).Name;
                                }
                                else
                                {
                                    cname += "," + cr.FirstOrDefault(a => a.Id.ToString() == cid[i]).Name;
                                }
                            }
                        }
                        item.Category = cname;
                    }//end if
                }
            }
            #endregion
            var ret = new BasePageResponse<List<TypeDataDefineData>>
            {
                Success = true,
                Message = "获取数据成功",
                Count = count,
                CurrentPage = req.PageNo,
                PageSize = req.PageSize,
                TotalPage = (int)Math.Ceiling((decimal)count / req.PageSize),
                Data = dtos
            };
            return ret;
        }
    }
}
