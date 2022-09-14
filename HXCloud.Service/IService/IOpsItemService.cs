using HXCloud.Model;
using HXCloud.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HXCloud.Service
{
    public interface IOpsItemService : IBaseService<OpsItemModel>
    {
        /// <summary>
        /// 添加巡检项目
        /// </summary>
        /// <param name="account">操作人</param>
        /// <param name="req">巡检项目</param>
        /// <returns></returns>
        Task<BaseResponse> AddOpsItemAsync(string account, OpsItemAddDto req);
        /// <summary>
        /// 更新巡检项目信息，key值不能更改
        /// </summary>
        /// <param name="account">操作人</param>
        /// <param name="req">巡检项目信息</param>
        /// <returns></returns>
        Task<BaseResponse> UpdateOpsItemAsync(string account, OpsItemUpdateDto req);
        /// <summary>
        /// 删除巡检项目
        /// </summary>
        /// <param name="account">操作人</param>
        /// <param name="Id">巡检项目标识</param>
        /// <returns></returns>
        Task<BaseResponse> DeleteOpsItemAsync(string account, int Id);
        /// <summary>
        /// 获取全部巡检项目
        /// </summary>
        /// <param name="req">查询条件</param>
        /// <returns></returns>
        Task<BaseResponse> GetOpsItemAsync(BaseRequest req);
        /// <summary>
        /// 获取分页巡检项目
        /// </summary>
        /// <param name="req">查询条件</param>
        /// <returns></returns>
        Task<BaseResponse> GetOpsItemPageAsync(BasePageRequest req);
    }
}
