using HXCloud.Model;
using HXCloud.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HXCloud.Service
{
    public interface ICraftElementService : IBaseService<CraftElementModle>
    {
        /// <summary>
        /// 添加工艺组件
        /// </summary>
        /// <param name="accout">操作人</param>
        /// <param name="userId">用户标识</param>
        /// <param name="catalogId">工艺组件类型标识</param>
        /// <param name="req">工艺组件数据</param>
        /// <returns></returns>
        Task<BaseResponse> AddCraftElementAsync(string accout, int userId, int catalogId, CraftElementAddDto req);

        /// <summary>
        /// 修改工艺组件
        /// </summary>
        /// <param name="account">操作人</param>
        /// <param name="path">文件路径</param>
        /// <param name="req">工艺组件数据</param>
        /// <returns></returns>
        Task<BaseResponse> EditCraftElementAsync(string account, string path, CraftElementEditDto req);
        /// <summary>
        /// 删除工艺组件，controller中需要删除图片
        /// </summary>
        /// <param name="accout">操作人</param>
        /// <param name="Id">工艺组件标识</param>
        /// <param name="path">文件路径</param>
        /// <returns></returns>
        Task<BaseResponse> DeleteCraftElementAsync(string accout, int Id, string path);


        Task<BaseResponse> GetCraftElementByIdAsync(int Id);
        Task<CraftElementExistDto> IsExistAsync(Expression<Func<CraftElementModle, bool>> predicate);
    }
}
