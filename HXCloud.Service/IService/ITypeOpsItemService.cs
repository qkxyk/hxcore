using HXCloud.Model;
using HXCloud.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HXCloud.Service
{
    public interface ITypeOpsItemService : IBaseService<TypeOpsItemModel>
    {
        /// <summary>
        /// 添加类型巡检项目
        /// </summary>
        /// <param name="account">操作人</param>
        /// <param name="req">巡检项目</param>
        /// <returns></returns>
        Task<BaseResponse> AddOpsItemAsync(string account, TypeOpsItemAddDto req);
        /// <summary>
        /// 更新巡检项目信息，key值不能更改
        /// </summary>
        /// <param name="account">操作人</param>
        /// <param name="req">巡检项目信息</param>
        /// <returns></returns>
        Task<BaseResponse> UpdateOpsItemAsync(string account, TypeOpsItemUpdateDto req);
        /// <summary>
        /// 删除巡检项目
        /// </summary>
        /// <param name="account">操作人</param>
        /// <param name="Id">巡检项目标识</param>
        /// <returns></returns>
        Task<BaseResponse> DeleteOpsItemAsync(string account, int Id);
        /// <summary>
        /// 获取全部类型巡检项目
        /// </summary>
        /// <param name="req">查询条件</param>
        /// <returns></returns>
        Task<BaseResponse> GetOpsItemAsync(BaseRequest req, int typeId);
        /// <summary>
        /// 获取分页类型巡检项目
        /// </summary>
        /// <param name="req">查询条件</param>
        /// <returns></returns>
        Task<BaseResponse> GetOpsItemPageAsync(BasePageRequest req, int typeId);
    }
}
