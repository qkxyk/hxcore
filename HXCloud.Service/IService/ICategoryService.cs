using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HXCloud.Model;
using HXCloud.ViewModel;

namespace HXCloud.Service
{
    public interface ICategoryService : IBaseService<CategoryModel>
    {
        Task<BaseResponse> AddCategroyAsync(string Account, CategoryAddDto req);
        Task<BaseResponse> UpdateCategoryAsync(string Account, CategoryUpdateDto req);
        Task<BaseResponse> DeleteCategoryAsync(string Account, int Id);
        Task<BaseResponse> GetCategroyAsync();
    }
}
