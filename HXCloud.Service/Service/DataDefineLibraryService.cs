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
    public class DataDefineLibraryService : IDataDefineLibraryService
    {
        private readonly ILogger<DataDefineLibraryService> _log;
        private readonly IDataDefineLibraryRepository _dlr;
        private readonly IMapper _mapper;
        private readonly ICategoryRepository _cr;

        public DataDefineLibraryService(ILogger<DataDefineLibraryService> log, IDataDefineLibraryRepository dlr, IMapper mapper, ICategoryRepository cr)
        {
            this._log = log;
            this._dlr = dlr;
            this._mapper = mapper;
            this._cr = cr;
        }
        public async Task<bool> IsExist(Expression<Func<DataDefineLibraryModel, bool>> predicate)
        {
            var ret = await _dlr.Find(predicate).FirstOrDefaultAsync();
            if (ret != null)
            {
                return true;
            }
            return false;
        }

        public async Task<BaseResponse> AddDataDefineAsync(DataDefineLibraryAddDto req, string account)
        {
            var data = await _dlr.Find(a => a.DataKey == req.DataKey || a.DataName == req.DataName).FirstOrDefaultAsync();
            if (data != null)
            {
                return new BaseResponse { Success = false, Message = "已存在相同的Key或者相同名称的数据定义" };
            }
            var entity = _mapper.Map<DataDefineLibraryModel>(req);
            try
            {
                entity.Create = account;
                await _dlr.AddAsync(entity);
                _log.LogInformation($"{account}添加数据定义库数据成功,添加的key为{req.DataKey}");
                return new HandleResponse<int>() { Success = true, Message = "添加数据成功", Key = entity.Id };
            }
            catch (Exception ex)
            {
                _log.LogError($"{account}添加key值为：{req.DataKey}数据定义库失败,失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "添加数据失败,请联系管理员" };
            }
        }

        public async Task<BaseResponse> UpdateDataDefineAsync(DataDefineLibraryUpdateDto req, string account)
        {
            var data = await _dlr.FindAsync(req.Id);
            if (data == null)
            {
                return new BaseResponse { Success = false, Message = "输入的编号不存在" };
            }
            //验证是否重名
            var ret = await _dlr.Find(a => a.DataName == req.DataName && a.Id != req.Id).FirstOrDefaultAsync();
            if (ret != null)
            {
                return new BaseResponse { Success = false, Message = "已存在相同名称的数据定义" };
            }
            var entity = _mapper.Map(req, data);
            try
            {
                entity.Modify = account;
                entity.ModifyTime = DateTime.Now;
                await _dlr.SaveAsync(data);
                _log.LogInformation($"{account}修改Id为{req.Id}数据定义库成功");
                return new BaseResponse { Success = true, Message = "修改数据成功" };
            }
            catch (Exception ex)
            {
                _log.LogError($"{account}修改Id为{req.Id}的数据定义库失败，失败原因{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "修改数据失败,请联系管理员" };
            }
        }

        public async Task<BaseResponse> DeleteDataDefineAsync(int Id, string account)
        {
            var ret = await _dlr.FindAsync(Id);
            if (ret == null)
            {
                return new BaseResponse { Success = false, Message = "输入的编号不存在" };
            }
            try
            {
                await _dlr.RemoveAsync(ret);
                _log.LogInformation($"{account}删除Id为{Id},Key值为{ret.DataKey}的数据定义库成功");
                return new BaseResponse { Success = true, Message = "删除数据成功" };
            }
            catch (Exception ex)
            {
                _log.LogError($"{account}删除Id为{Id}的数据定义库失败，失败原因{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "删除数据失败,请联系管理员" };
            }
        }

        public async Task<BaseResponse> GetDataDefineLibraryAsync(int Id)
        {
            var data = await _dlr.FindAsync(Id);
            if (data == null)
            {
                return new BaseResponse { Success = false, Message = "输入的编号不存在" };
            }
            var dto = _mapper.Map<DataDefineLibraryDataDto>(data);
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
            return new BResponse<DataDefineLibraryDataDto>() { Success = true, Message = "获取数据成功", Data = dto };
        }

        public async Task<BaseResponse> GetDataDefineLibrarysAsync(DataDefineLibraryPageRequest req)
        {
            var ret = _dlr.Find(a => true);

            if (!string.IsNullOrWhiteSpace(req.Search))
            {
                ret = ret.Where(a => a.DataKey.Contains(req.Search) || a.DataName.Contains(req.Search));
            }
            //单个标签查找，第一步先查找包含的id，第二步查找id包含的，如果是多个标签的或查找需要根据标签或
            //Regex r = new Regex(@"(?=(,|\b)1\b)|(?=(,|\b)2\b)")
            if (req.CategoryId != 0)
            {
                string txt = $@"(?=(,|\b){req.CategoryId}\b)";
                Regex r = new Regex(@txt);
                var Ids = _dlr.Find(a => true).ToList().Where(a =>
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
                ret = ret.Where(a => Ids.Contains(a.Id));
            }
            int Count = 0;
            string OrderExpression = "";
            if (string.IsNullOrEmpty(req.OrderBy))
            {
                OrderExpression = "Id Asc";
            }
            else
            {
                var orderExpression = string.Format("{0} {1}", req.OrderBy, req.OrderType);
            }
            Count = ret.Count();

            var data = await ret.OrderBy(OrderExpression).Skip((req.PageNo - 1) * req.PageSize).Take(req.PageSize).ToListAsync();
            var dto = _mapper.Map<IEnumerable<DataDefineLibraryDataDto>>(data);
            #region 替换category
            var cr = await _cr.Find(a => true).ToListAsync();
            if (cr.Count > 0)
            {
                foreach (var item in dto)
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
            BaseResponse br = new BasePageResponse<IEnumerable<DataDefineLibraryDataDto>>()
            {
                Count = Count,
                CurrentPage = req.PageNo,
                PageSize = req.PageSize,
                Success = true,
                Message = "获取数据成功",
                Data = dto,
                TotalPage = (int)Math.Ceiling((decimal)Count / req.PageSize)
            };
            return br;
        }
    }
}
