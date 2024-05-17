using AutoMapper;
using HXCloud.Model;
using HXCloud.Repository;
using HXCloud.ViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HXCloud.Service
{
    public class CraftComponentCatalogService : ICraftComponentCatalogService
    {
        private readonly ILogger<CraftComponentCatalogService> _log;
        private readonly IMapper _mapper;
        private readonly ICraftComponentCatalogRepository _craftComponentCatelog;

        public CraftComponentCatalogService(ILogger<CraftComponentCatalogService> log, IMapper mapper, ICraftComponentCatalogRepository craftComponentCatelog)
        {
            this._log = log;
            this._mapper = mapper;
            this._craftComponentCatelog = craftComponentCatelog;
        }
        public async Task<bool> IsExist(Expression<Func<CraftComponentCatalogModle, bool>> predicate)
        {
            var data = await _craftComponentCatelog.Find(predicate).FirstOrDefaultAsync();
            if (data == null)
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 是否存在组件类型
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public async Task<CatalogExistDto> IsExistWithElementAsync(Expression<Func<CraftComponentCatalogModle, bool>> predicate)
        {
            CatalogExistDto dto = new CatalogExistDto();
            var data = await _craftComponentCatelog.GetWithElementAsync(predicate);
            if (data == null)
            {
                dto.IsExist = false;
            }
            else
            {
                dto.IsExist = true;
                if (data.ElementType == CraftType.Open)
                {
                    dto.CraftType = 0;
                }
                else
                {
                    dto.CraftType = 1;
                }
                if (data.CraftCatalogType == CraftCatalogType.Leaf)
                {
                    dto.IsLeaf = true;
                }
                else
                {
                    if (data.Child.Count > 0)
                    {
                        dto.HasChild = true;
                    }
                }
                if (data.CraftElements.Count > 0)
                {
                    dto.HasExistElement = true;
                }
            }

            return dto;
        }

        /// <summary>
        /// 添加工艺组件类型
        /// </summary>
        /// <param name="account">操作人</param>
        /// <param name="req">工艺组件数据</param>
        /// <returns></returns>
        public async Task<BaseResponse> AddCraftComponentCatalogAsync(string account, CraftComponentCatalogAddDto req)
        {
            //验证是否存在
            var data = await _craftComponentCatelog.Find(a => a.Name == req.Name && a.ParentId == req.ParentId).FirstOrDefaultAsync();
            if (data != null)
            {
                return new BaseResponse { Success = false, Message = "已存在相同名称的工艺组件类型" };
            }
            try
            {
                var entity = _mapper.Map<CraftComponentCatalogModle>(req);
                entity.Create = account;
                if (req.ParentId.HasValue)
                {
                    entity.CraftCatalogType = CraftCatalogType.Leaf;
                }
                await _craftComponentCatelog.AddAsync(entity);
                _log.LogInformation($"{account}添加标识为{entity.Id}的工艺组件类型成功");
                return new HandleIdResponse<int> { Id = entity.Id, Message = "添加工艺组件类型成功", Success = true };
            }
            catch (Exception ex)
            {
                _log.LogError($"{account}添加名称为{req.Name}的组件类型失败，失败原因:{ex.Message}->{ex.InnerException}->{ex.StackTrace}");
                return new BaseResponse { Success = false, Message = "添加工艺组件失败，请联系管理员" };
            }
        }

        /// <summary>
        /// 修改工艺组件类型
        /// </summary>
        /// <param name="account">操作人</param>
        ///<param name="path">文件路径</param>
        /// <param name="req">工艺组件数据</param>
        /// <returns></returns>
        public async Task<BaseResponse> EditCraftComponentCatalogAsync(string account, string path, CraftComponentCatalogEditDto req)
        {
            var data = await _craftComponentCatelog.FindAsync(req.Id);
            if (data == null)
            {
                return new BaseResponse { Success = false, Message = "输入的工艺组件分类不存在" };
            }
            try
            {
                string old = data.Icon;
                var entity = _mapper.Map(req, data);
                entity.Modify = account;
                entity.ModifyTime = DateTime.Now;
                await _craftComponentCatelog.SaveAsync(entity);
                _log.LogInformation($"{account}修改标识为{req.Id}的工艺组件类型成功");
                //删除原有的图标
                if (!string.IsNullOrEmpty(old))
                {
                    string url = Path.Combine(path, old);
                    if (File.Exists(url))
                    {
                        File.Delete(url);
                    }
                }
                return new BaseResponse { Success = true, Message = "修改数据成功" };
            }
            catch (Exception ex)
            {
                _log.LogError($"{account}修改标识为{req.Id}的工艺组件类型失败,失败原因：{ex.Message}->{ex.InnerException}->{ex.StackTrace}");
                return new BaseResponse { Success = false, Message = "修改工艺组件类型失败，请联系管理员" };
            }
        }
        /// <summary>
        /// 删除工艺组件类型数据
        /// </summary>
        /// <param name="account">操作人</param>
        /// <param name="Id">工艺组件类型标识</param>
        /// <param name="path">文件路径</param>
        /// <returns></returns>
        public async Task<BaseResponse> DeleteCraftComponentCatalogAsync(string account, int Id, string path)
        {
            var data = await _craftComponentCatelog.FindAsync(Id);
            if (data == null)
            {
                return new BaseResponse { Success = false, Message = "输入的工艺组件分类不存在" };
            }
            try
            {
                //先删除文件
                if (!string.IsNullOrEmpty(data.Icon))
                {
                    string url = Path.Combine(path, data.Icon);
                    if (System.IO.File.Exists(url))
                    {
                        System.IO.File.Delete(url);
                    }
                }

                await _craftComponentCatelog.RemoveAsync(data);
                _log.LogInformation($"{account}删除标识为{Id}的工艺组件类型成功");
                return new BaseResponse { Success = true, Message = "删除工艺组件类型成功" };
            }
            catch (Exception ex)
            {
                _log.LogError($"{account}删除标识为{Id}的工艺组件类型失败，失败原因:{ex.Message}->{ex.InnerException}->{ex.StackTrace}");
                return new BaseResponse { Success = false, Message = "删除工艺组件类型失败，请联系管理员" };
            }
        }

        /// <summary>
        /// 根据组件类型标识获取工艺组件类型
        /// </summary>
        /// <param name="Id">工艺组件类型标识</param>
        /// <returns></returns>
        public async Task<BaseResponse> GetCraftComponentCatalogAsync(int Id)
        {
            var data = await _craftComponentCatelog.FindAsync(Id);
            if (data == null)
            {
                return new BaseResponse { Success = false, Message = "输入的工艺组件类型不存在" };
            }
            var dto = _mapper.Map<CraftComponentCatalogDto>(data);
            return new BResponse<CraftComponentCatalogDto> { Success = true, Message = "获取数据成功", Data = dto };
        }

        /*
        /// <summary>
        /// 获取全部工艺组件类型
        /// </summary>
        /// <returns></returns>
        public async Task<BaseResponse> GetAllCraftComponentCatalogAsync()
        {
            var data = _craftComponentCatelog.Find(a => true).ToListAsync();
            var dtos = _mapper.Map<List<CraftComponentCatalogDto>>(data);
            return new BResponse<List<CraftComponentCatalogDto>> { Success = true, Message = "获取数据成功", Data = dtos };
        }
*/
        /// <summary>
        /// 获取用户能查到的所有工艺组件
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="isAdmin"></param>
        /// <returns></returns>
        public async Task<BaseResponse> GetMyCraftComponentCatalogAsync(int userId, bool isAdmin)
        {
            //获取全部数据
            var data = await _craftComponentCatelog.GetAll(a => a.CraftCatalogType == CraftCatalogType.Root);
            if (!isAdmin)
            {
                data.ForEach(a =>
                {
                    //排除不是自己的工艺组件
                    var t = a.CraftElements.Where(a => a.ElementType == CraftType.Personal && a.UserId != userId).ToList();
                    for (int i = 0; i < t.Count; i++)
                    {
                        a.CraftElements.Remove(t[i]);
                    }
                });
            }
            var dto = _mapper.Map<List<CraftComponentCatalogDto>>(data);
            return new BResponse<List<CraftComponentCatalogDto>> { Success = true, Message = "获取数据成功", Data = dto };
        }
    }
}
