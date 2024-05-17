using HXCloud.Model;
using HXCloud.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HXCloud.Service
{
    public interface ICraftComponentCatalogService : IBaseService<CraftComponentCatalogModle>
    {
        /// <summary>
        /// 添加工艺组件类型
        /// </summary>
        /// <param name="account">操作人</param>
        /// <param name="req">工艺组件数据</param>
        /// <returns></returns>
        public Task<BaseResponse> AddCraftComponentCatalogAsync(string account, CraftComponentCatalogAddDto req);

        /// <summary>
        /// 修改工艺组件类型
        /// </summary>
        /// <param name="account">操作人</param>
        ///<param name="path">文件路径</param>
        /// <param name="req">工艺组件数据</param>
        /// <returns></returns>
        Task<BaseResponse> EditCraftComponentCatalogAsync(string account, string path, CraftComponentCatalogEditDto req);
        /// <summary>
        /// 删除工艺组件类型数据
        /// </summary>
        /// <param name="account">操作人</param>
        /// <param name="Id">工艺组件类型标识</param>
        /// <param name="path">文件路径</param>
        /// <returns></returns>
        public Task<BaseResponse> DeleteCraftComponentCatalogAsync(string account, int Id, string path);

        /// <summary>
        /// 根据组件类型标识获取工艺组件类型
        /// </summary>
        /// <param name="Id">工艺组件类型标识</param>
        /// <returns></returns>
        public Task<BaseResponse> GetCraftComponentCatalogAsync(int Id);
        /*
                /// <summary>
                /// 获取全部工艺组件类型
                /// </summary>
                /// <returns></returns>
                public Task<BaseResponse> GetAllCraftComponentCatalogAsync();
        */
        /// <summary>
        /// 获取用户能查到的所有工艺组件
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="isAdmin"></param>
        /// <returns></returns>
        public Task<BaseResponse> GetMyCraftComponentCatalogAsync(int userId, bool isAdmin);
        /// <summary>
        /// 是否存在组件类型
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<CatalogExistDto> IsExistWithElementAsync(Expression<Func<CraftComponentCatalogModle, bool>> predicate);
    }
}
