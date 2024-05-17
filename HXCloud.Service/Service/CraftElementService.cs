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
    public class CraftElementService : ICraftElementService
    {
        private readonly ILogger<CraftElementService> _log;
        private readonly IMapper _mapper;
        private readonly ICraftComponentCatalogRepository _craftComponentCatalog;
        private readonly ICraftElementRepository _craftElement;

        public CraftElementService(ILogger<CraftElementService> log, IMapper mapper, ICraftComponentCatalogRepository craftComponentCatalog, ICraftElementRepository craftElement)
        {
            this._log = log;
            this._mapper = mapper;
            this._craftComponentCatalog = craftComponentCatalog;
            this._craftElement = craftElement;
        }
        public async Task<bool> IsExist(Expression<Func<CraftElementModle, bool>> predicate)
        {
            var data = await _craftElement.Find(predicate).FirstOrDefaultAsync();
            if (data==null)
            {
                return false;
            }
            return true;
        }
        public async Task<CraftElementExistDto> IsExistAsync(Expression<Func<CraftElementModle,bool>> predicate)
        {
            var data = await _craftElement.Find(predicate).FirstOrDefaultAsync();
            if (data==null)
            {
                return null;
            }
            return new CraftElementExistDto { ElementType = (int)data.ElementType, UserId = data.UserId ?? 0 };
        }
        /// <summary>
        /// 添加工艺组件
        /// </summary>
        /// <param name="accout">操作人</param>
        /// <param name="userId">用户标识</param>
        /// <param name="catalogId">工艺组件类型标识</param>
        /// <param name="req">工艺组件数据</param>
        /// <returns></returns>
        public async Task<BaseResponse> AddCraftElementAsync(string accout, int userId, int catalogId, CraftElementAddDto req)
        {
            var data = await _craftElement.Find(a => a.CatalogId == catalogId && a.Name == req.Name).FirstOrDefaultAsync();
            if (data != null)
            {
                return new BaseResponse { Success = false, Message = "已添加过相同名称的工艺组件" };
            }
            try
            {
                var entity = _mapper.Map<CraftElementModle>(req);
                entity.Create = accout;
                entity.CatalogId = catalogId;
                if (entity.ElementType == CraftType.Personal)//个人组件添加用户标识
                {
                    entity.UserId = userId;
                }
                await _craftElement.AddAsync(entity);
                _log.LogInformation($"{accout}添加标识为{entity.Id}的工艺组件成功");
                return new HandleIdResponse<int> { Id = entity.Id, Success = true, Message = "添加数据成功" };
            }
            catch (Exception ex)
            {
                _log.LogError($"{accout}添加工艺组件失败，失败原因:{ex.Message}->{ex.InnerException}->{ex.StackTrace}");
                return new BaseResponse { Success = false, Message = "添加数据失败，请联系管理员" };
            }
        }

        /// <summary>
        /// 修改工艺组件
        /// </summary>
        /// <param name="account">操作人</param>
        /// <param name="path">文件路径</param>
        /// <param name="req">工艺组件数据</param>
        /// <returns></returns>
        public async Task<BaseResponse> EditCraftElementAsync(string account, string path, CraftElementEditDto req)
        {
            var data = await _craftElement.FindAsync(req.Id);
            if (data == null)
            {
                return new BaseResponse { Success = false, Message = "输入的工艺组件不存在" };
            }
            try
            {
                string old = data.Image;
                var entity = _mapper.Map(req, data);
                entity.Modify = account;
                entity.ModifyTime = DateTime.Now;
                await _craftElement.SaveAsync(entity);
                _log.LogInformation($"{account}修改标识为{req.Id}的工艺组件成功");
                //删除原有的图标
                if (path!=null)
                {
                    if (!string.IsNullOrEmpty(old))
                    {
                        string url = Path.Combine(path, old);
                        if (File.Exists(url))
                        {
                            File.Delete(url);
                        }
                    }
                }             
                return new BaseResponse { Success = true, Message = "修改数据成功" };
            }
            catch (Exception ex)
            {
                _log.LogError($"{account}修改标识为{req.Id}的工艺组件失败,失败原因：{ex.Message}->{ex.InnerException}->{ex.StackTrace}");
                return new BaseResponse { Success = false, Message = "修改工艺组件失败，请联系管理员" };
            }
        }
        /// <summary>
        /// 删除工艺组件，controller中需要删除图片
        /// </summary>
        /// <param name="accout">操作人</param>
        /// <param name="Id">工艺组件标识</param>
        /// <param name="path">文件路径</param>
        /// <returns></returns>
        public async Task<BaseResponse> DeleteCraftElementAsync(string accout, int Id, string path)
        {
            var data = await _craftElement.FindAsync(Id);
            if (data == null)
            {
                return new BaseResponse { Success = false, Message = "输入的工艺组件编号不存在" };
            }
            try
            {
                //先删除文件
                if (!string.IsNullOrEmpty(data.Image))
                {
                    string url = Path.Combine(path, data.Image);
                    if (File.Exists(url))
                    {
                        File.Delete(url);
                    }
                }
                await _craftElement.RemoveAsync(data);
                _log.LogInformation($"{accout}删除标识为{Id}的工艺组件成功");
                return new BaseResponse { Success = true, Message = "删除数据成功" };
            }
            catch (Exception ex)
            {
                _log.LogError($"{accout}删除标识为{Id}的工艺组件失败，失败原因:{ex.Message}->{ex.InnerException}->{ex.StackTrace}");
                return new BaseResponse { Success = false, Message = "删除工艺组件失败，请联系管理员" };
            }
        }

        public async Task<BaseResponse> GetCraftElementByIdAsync(int Id)
        {
            var data = await _craftElement.FindAsync(Id);
            if (data==null)
            {
                return new BaseResponse { Success = false, Message = "输入的工艺组件编号不存在" };
            }
            var dto = _mapper.Map<CraftElementDto>(data);
            return new BResponse<CraftElementDto> { Success = true, Message = "获取数据成功", Data = dto };
        }

        /*
        public async Task<BaseResponse> GetCraftElementByCatalogIdAsync(int catalogId)
        {
            var data = await
        }
        */
    }
}
