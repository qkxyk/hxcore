using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HXCloud.Model;
using HXCloud.ViewModel;

namespace HXCloud.Service
{
    /// <summary>
    /// 主要用来显示类型关联的设备的总揽数据（根据接收的mqtt数据解析数据显示当前设备的状态数据）
    /// </summary>
    public interface ITypeOverviewService : IBaseService<TypeOverviewModel>
    {
        /// <summary>
        /// 添加类型总揽数据
        /// </summary>
        /// <param name="Account">添加人</param>
        /// <param name="TypeId">类型标示</param>
        /// <param name="req">类型总揽数据</param>
        /// <returns></returns>
        Task<BaseResponse> AddTypeOverviewAsync(string Account, int TypeId, TypeOverViewAddDto req);
        /// <summary>
        /// 修改类型总揽数据，同一个类型下名称不能重复
        /// </summary>
        /// <param name="Account">修改人</param>
        /// <param name="TypeId">类型标示</param>
        /// <param name="req">类型总揽数据</param>
        /// <returns></returns>
        Task<BaseResponse> UpdateTypeOverviewAsync(string Account, int TypeId, TypeOverviewUpdateDto req);
        /// <summary>
        /// 删除类型总揽数据
        /// </summary>
        /// <param name="Account">操作人</param>
        /// <param name="Id">类型总揽标示</param>
        /// <returns></returns>
        Task<BaseResponse> DeleteTypeOverviewAsync(string Account, int Id);
        /// <summary>
        /// 获取类型的总揽数据，获取全部
        /// </summary>
        /// <param name="TypeId">类型标示</param>
        /// <returns></returns>
        Task<BaseResponse> GetTypeOverviewByTypeIdAsync(int TypeId);
    }
}
