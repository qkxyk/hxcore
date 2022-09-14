using HXCloud.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HXCloud.Repository
{
    public class IssueRepository : BaseRepository<IssueModel>, IIssueRepository
    {
        /// <summary>
        /// 获取用户提交的问题单
        /// </summary>
        /// <param name="account">用户列表</param>
        /// <returns></returns>
        public IQueryable<IssueModel> GetIssue(List<string> account)
        {
            var data = _db.Issues.Where(a => account.Contains(a.Create));
            return data;
        }
    }
}
