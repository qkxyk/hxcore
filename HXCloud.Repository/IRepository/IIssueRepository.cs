using HXCloud.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HXCloud.Repository
{
    public interface IIssueRepository:IBaseRepository<IssueModel>
    {
        /// <summary>
        /// 获取用户提交的问题单
        /// </summary>
        /// <param name="account">用户列表</param>
        /// <returns></returns>
        IQueryable<IssueModel> GetIssue(List<string> account);
    }
}
