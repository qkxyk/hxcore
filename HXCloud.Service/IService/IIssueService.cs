using HXCloud.Model;
using HXCloud.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HXCloud.Service
{
    public interface IIssueService : IBaseService<IssueModel>
    {
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
        /// <param name="isAdmin">是否管理员</param>
        ///<param name="category">运维人员标识</param>
        ///<param name="path">图片保存的目录路径</param>
        /// <returns></returns>
        Task<BaseResponse> DeleteIssueAsync(string account, int Id, bool isAdmin, int category, string path);
        /// <summary>
        /// 获取用户的问题单
        /// </summary>
        /// <param name="account">用户列表</param>
        /// <param name="req">用户的请求数据，可以根据设备的名称查找</param>
        /// <returns>返回用户的问题单</returns>
        Task<BasePageResponse<List<IssueDto>>> GetIssuePageRequestAsync(List<string> account, IssuePageRequest req);
        /// <summary>
        /// 获取问题单数据
        /// </summary>
        /// <param name="Id">问题单编号</param>
        /// <returns></returns>
        Task<IssueDto> GetIssueByIdAsync(int Id);
        /// <summary>
        /// 根据问题单编号获取问题单信息
        /// </summary>
        /// <param name="Id">问题单编号</param>
        /// <param name="users">用户列表</param>
        /// <returns></returns>
        Task<BaseResponse> GetIssueByIdAsync(int Id, List<string> users);
    }
}
