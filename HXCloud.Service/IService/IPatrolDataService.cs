using HXCloud.Model;
using HXCloud.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HXCloud.Service
{
    public interface IPatrolDataService : IBaseService<PatrolDataModel>
    {
        /// <summary>
        /// 查询巡检单
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <returns></returns>
        Task<PatrolDataModel> IsExistAsync(Expression<Func<PatrolDataModel, bool>> predicate);
        /// <summary>
        /// 创建巡检单
        /// </summary>
        /// <param name="account">操作人</param>
        /// <param name="Id">操作人标识，用来防止同一个用户在短时间内频繁创建巡检单</param>
        /// <param name="req">巡检单内容</param>
        /// <returns></returns>
        Task<BaseResponse> AddPatrolDataAsync(string account, int Id, PatrolDataAddDto req);
        /// <summary>
        /// 添加巡检图片
        /// </summary>
        /// <param name="account">操作人</param>
        /// <param name="req">图片地址</param>
        /// <returns></returns>
        Task<BaseResponse> AddPatrolImageAsync(string account, PatrolImageAddDto req);
        /// <summary>
        /// 添加巡检数据
        /// </summary>
        /// <param name="account">操作人</param>
        /// <param name="req">巡检数据</param>
        /// <returns></returns>
        Task<BaseResponse> AddPatrolItemAsync(string account, PatrolItemAddDto req);
        /// <summary>
        /// 删除巡检数据
        /// </summary>
        /// <param name="account">操作人</param>
        /// <param name="Id">巡检单号</param>
        ///<param name="path">图片保存的目录路径</param>
        /// <returns>删除成功后需要处理巡检图片</returns>
        Task<BaseResponse> DeletePatrolDataAsync(string account, string Id, string path);
        /// <summary>
        ///获取巡检项目,需要和类型巡检项目合并 
        /// </summary>
        /// <param name="req">巡检类型</param>
        /// <param name="typeId">设备类型标识</param>
        /// <returns></returns>
        Task<BaseResponse> GetPatrolItemAsync(PatrolItemRequest req, int typeId);
        [Obsolete(message: "错误，请调用GetPagePatrolDataAsync")]
        /// <summary>
        /// 获取巡检数据
        /// </summary>
        /// <param name="users">用户列表</param>
        /// <param name="req">查询条件</param>
        /// <returns>返回用户巡检数据</returns>
        Task<BaseResponse> GetPatrolDataPageAsync(List<string> users, PatrolDataRequest req);
        /// <summary>
        ///获取用户的分页巡检单, 查询三种类型，管理员权限，查询个人的，根据用户角色查询的
        /// </summary>
        /// <param name="req">查询条件</param>
        /// <param name="isAdmin">是否管理员</param>
        /// <param name="account">非管理员没有查询权限的查找自己</param>
        /// <param name="DeviceSn">非管理员有查询权限查看的设备列表</param>
        /// <returns></returns>
        Task<BaseResponse> GetPagePatrolDataAsync(PatrolDataRequest req, bool isAdmin, string account, List<string> DeviceSn);

        /// <summary>
        /// 根据巡检单号获取巡检数据
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<BaseResponse> GetPatrolDataByIdAsync(string Id);
    }
}
