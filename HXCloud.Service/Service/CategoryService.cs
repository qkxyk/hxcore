using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using HXCloud.Model;
using HXCloud.Repository;
using HXCloud.ViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace HXCloud.Service
{
    public class CategoryService : ICategoryService
    {
        private readonly ILogger<CategoryService> _log;
        private readonly IMapper _map;
        private readonly ICategoryRepository _cr;
        private readonly IDataDefineLibraryRepository _dl;
        private readonly ITypeDataDefineRepository _tr;

        public CategoryService(ILogger<CategoryService> log, IMapper map, ICategoryRepository cr, IDataDefineLibraryRepository dl, ITypeDataDefineRepository tr)
        {
            this._log = log;
            this._map = map;
            this._cr = cr;
            this._dl = dl;
            this._tr = tr;
        }
        public Task<bool> IsExist(Expression<Func<CategoryModel, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public async Task<BaseResponse> AddCategroyAsync(string Account, CategoryAddDto req)
        {
            var cate = await _cr.Find(a => a.Name == req.Name).FirstOrDefaultAsync();
            if (cate != null)
            {
                return new BaseResponse { Success = false, Message = "已存在相同名称的分类" };
            }
            try
            {
                var entity = _map.Map<CategoryModel>(req);
                entity.Create = Account;
                await _cr.AddAsync(entity);
                _log.LogInformation($"{Account}添加标示为{entity.Id}的分类成功");
                return new HandleResponse<int> { Success = true, Message = "添加数据成功", Key = entity.Id };
            }
            catch (Exception ex)
            {
                _log.LogError($"{Account}添加分类失败，失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "添加数据失败，请联系管理员" };
            }
        }
        public async Task<BaseResponse> UpdateCategoryAsync(string Account, CategoryUpdateDto req)
        {
            var cate = await _cr.FindAsync(req.Id);
            if (cate == null)
            {
                return new BaseResponse { Success = false, Message = "输入的分类标示不存在" };
            }
            try
            {
                var dto = _map.Map(req, cate);
                dto.Modify = Account;
                dto.ModifyTime = DateTime.Now;
                await _cr.SaveAsync(dto);
                _log.LogInformation($"{Account}修改标示为{req.Id}的分类数据成功");
                return new BaseResponse { Success = true, Message = "修改数据成功" };
            }
            catch (Exception ex)
            {
                _log.LogError($"{Account}修改标示为:{req.Id}的分类数据失败，失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "修改数据失败，请联系管理员" };
            }
        }
        public async Task<BaseResponse> DeleteCategoryAsync(string Account, int Id)
        {
            var cate = await _cr.FindAsync(Id);
            if (cate == null)
            {
                return new BaseResponse { Success = false, Message = "输入的分类标示不存在" };
            }
            //查找datadefinelibrary和typedatadefine中是否使用
            var dl = await _dl.Find(a => a.Category != null).ToListAsync();
            var dle = dl.Where(a => a.Category.Split(',').Contains(Id.ToString()));
            if (dle.Count() > 0)
            {
                return new BaseResponse { Success = false, Message = "数据定义库中存在使用该分类标示的数据，不能删除" };
            }
            var dt = await _tr.Find(a => a.Category != null).ToListAsync();
            var tle = dt.Where(a => a.Category.Split(',').Contains(Id.ToString()));
            if (tle.Count() > 0)
            {
                return new BaseResponse { Success = false, Message = "类型数据定义中存在使用该分类标示的数据，不能删除" };
            }
            try
            {
                await _cr.RemoveAsync(cate);
                _log.LogInformation($"{Account}删除标示为{Id}的分类数据成功");
                return new BaseResponse { Success = true, Message = "删除分类数据成功" };
            }
            catch (Exception ex)
            {
                _log.LogError($"{Account}删除标示为{Id}的分类数据失败，失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "删除数据失败，请联系管理员" };
            }
        }
        public async Task<BaseResponse> GetCategroyAsync()
        {
            var data = await _cr.Find(a => true).ToListAsync();
            var dtos = _map.Map<IEnumerable<CategoryDto>>(data);
            return new BResponse<IEnumerable<CategoryDto>> { Success = true, Message = "获取数据成功", Data = dtos };
        }
    }
}
