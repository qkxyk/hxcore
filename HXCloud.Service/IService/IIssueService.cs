using HXCloud.Model;
using HXCloud.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HXCloud.Service
{
    public interface IIssueService : IBaseService<IssueModel>
    {
        /// <summary>
        /// 查询问题单
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <returns></returns>
        Task<IssueModel> IsExistAsync(Expression<Func<IssueModel, bool>> predicate);
        /// <summary>
        /// 添加上报问题
        /// </summary>
        /// <param name="account">操作人</param>
        /// <param name="req">上报数据</param>
        /// <returns>返回操作是否成功</returns>
        Task<BaseResponse> AddIssueAsync(string account, IssueAddDto req);

        /// <summary>
        /// 处理问题单，已处理的问题单不能重复处理
        /// </summary>
        /// <param name="account">处理人</param>
        /// <param name="req">问题单信息</param>
        /// <returns></returns>
        Task<BaseResponse> UpdateIssueAsync(string account, IssueUpdateDto req);
        /// <summary>
        /// 删除问题单
        /// </summary>
        /// <param name="account">操作人</param>
        /// <param name="Id">问题单标识</param>
        ///<param name="path">图片保存的目录路径</param>
        /// <returns></returns>
        Task<BaseResponse> DeleteIssueAsync(string account, int Id, string path);
        [Obsolete("过时的方法，请使用GetPageIssueAsync")]
        /// <summary>
        /// 获取用户的问题单
        /// </summary>
        /// <param name="account">用户列表</param>
        /// <param name="req">用户的请求数据，可以根据设备的名称查找</param>
        /// <returns>返回用户的问题单</returns>
        Task<BasePageResponse<List<IssueDto>>> GetIssuePageRequestAsync(List<string> account, IssuePageRequest req);
        /// <summary>
        ///获取用户的分页问题单, 查询三种类型，管理员权限，查询个人的，根据用户角色查询的
        /// </summary>
        /// <param name="req">查询条件</param>
        /// <param name="isAdmin">是否管理员</param>
        /// <param name="account">非管理员没有查询权限的查找自己</param>
        /// <param name="DeviceSn">非管理员有查询权限查看的设备列表</param>
        /// <returns></returns>
        Task<BaseResponse> GetPageIssueAsync(IssuePageRequest req, bool isAdmin, string account, List<string> DeviceSn);
        /// <summary>
        /// 获取问题单数据
        /// </summary>
        /// <param name="Id">问题单编号</param>
        /// <returns></returns>
        Task<BaseResponse> GetIssueByIdAsync(int Id);
        [Obsolete]
        /// <summary>
        /// 根据问题单编号获取问题单信息
        /// </summary>
        /// <param name="Id">问题单编号</param>
        /// <param name="users">用户列表</param>
        /// <returns></returns>
        Task<BaseResponse> GetIssueByIdAsync(int Id, List<string> users);
    }
}
