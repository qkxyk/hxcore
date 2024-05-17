using HXCloud.Model;
using HXCloud.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HXCloud.Service
{
    public interface IRepairService : IBaseService<RepairModel>
    {
        /// <summary>
        /// 下发维修或者调试单
        /// </summary>
        /// <param name="account">操作人</param>
        /// <param name="Id">操作人标识</param>
        /// <param name="req">维修或者操作信息</param>
        /// <returns></returns>
        Task<BaseResponse> AddRepairAsync(string account, int Id, RepairAddDto req);
        /// <summary>
        /// 查找维修信息
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <returns></returns>
        Task<RepairModel> IsExistAsync(Expression<Func<RepairModel, bool>> predicate);
        /// <summary>
        /// 运维单接单
        /// </summary>
        /// <param name="Id">运维单编号</param>
        /// <param name="Account">接单人</param>
        /// <param name="status">运维状态</param>
        /// <returns></returns>
        //Task<BaseResponse> ReceiveRepairAsync(string Id, string Account, int status);
        /// 设置运维单为等待配件状态
        /// </summary>
        /// <param name="Id">运维单编号</param>
        /// <param name="Account">接单人</param>
        /// <param name="status">运维状态</param>
        /// <returns></returns>
        //Task<BaseResponse> WaitRepairAsync(string Id, string Account, int status);
        /// <summary>
        /// 上传调试或者维修数据
        /// </summary>
        /// <param name="account">操作人</param>
        /// <param name="req">调试或者维修数据</param>
        /// <returns></returns>
        //Task<BaseResponse> UploadRepairImageAsync(string account, RepairAddImageDto req);
        /// <summary>
        /// 审核运维单
        /// </summary>
        /// <param name="account">操作人</param>
        /// <param name="req">审核数据</param>
        /// <returns></returns>
        //Task<BaseResponse> CheckRepairAsync(string account, RepairCheckDto req);
        /// <summary>
        /// 删除运维单，只能删除状态为未接单的
        /// </summary>
        /// <param name="account">操作人</param>
        /// <param name="Id">运维单编号</param>
        /// <returns></returns>
        Task<BaseResponse> DeleteRepairAsync(string account, string Id);
        [Obsolete("该方法已被弃用，请使用重载的GetRepairAsync代替")]
        /// <summary>
        /// 获取用户的运维单
        /// </summary>
        /// <param name="req">运维单状态和类型</param>
        /// <returns></returns>
        Task<BaseResponse> GetRepairAsync(RepairRequestDto req);
        /// <summary>
        /// 查询三种类型，管理员权限，查询个人的，根据用户角色查询的
        /// </summary>
        /// <param name="req">查询条件</param>
        /// <param name="isAdmin">是否管理员</param>
        /// <param name="account">非管理员没有查询权限的查找自己</param>
        /// <param name="DeviceSn">非管理员有查询权限查看的设备列表</param>
        /// <returns></returns>
        Task<BaseResponse> GetRepairAsync(RepairRequest req, bool isAdmin, string account, List<string> DeviceSn);
        /// <summary>
        /// 获取用户的分页运维单
        /// </summary>
        /// <param name="req">运维单状态和类型</param>
        /// <returns></returns>
        Task<BaseResponse> GetPageRepairAsync(RepairPageRequestDto req);
        /// <summary>
        /// 获取维修单或者调试单和关联的问题单信息
        /// </summary>
        /// <param name="account">用户，只有维修单的下发人和接单人有权限查看</param>
        /// <param name="Id">单据编号</param>
        /// <returns></returns>
        Task<BaseResponse> GetRepairByIdAsync(string account, string Id);

        /// <summary>
        /// 获取维修单或者调试单和关联的问题单信息
        /// </summary>
        /// <param name="Id">单据编号</param>
        /// <returns></returns>
        Task<BaseResponse> GetRepairByIdAsync(string Id);
        /// <summary>
        ///获取用户的分页运维单, 查询三种类型，管理员权限，查询个人的，根据用户角色查询的
        /// </summary>
        /// <param name="req">查询条件</param>
        /// <param name="isAdmin">是否管理员</param>
        /// <param name="account">非管理员没有查询权限的查找自己</param>
        /// <param name="DeviceSn">非管理员有查询权限查看的设备列表</param>
        /// <returns></returns>
       Task<BaseResponse> GetPageRepairAsync(RepairPageRequest req, bool isAdmin, string account, List<string> DeviceSn);
    }
}
