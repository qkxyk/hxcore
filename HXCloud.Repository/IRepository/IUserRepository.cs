using HXCloud.Model;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HXCloud.Repository
{
    public interface IUserRepository : IBaseRepository<UserModel>
    {
        Task<UserModel> FindWithGroup(Expression<Func<UserModel, bool>> predicate);
        /// <summary>
        /// 获取用户和用户角色信息
        /// </summary>
        /// <param name="predicate">用户的查询条件</param>
        /// <returns></returns>
        Task<UserModel> FindWithRoleAsync(Expression<Func<UserModel, bool>> predicate);
    }
}
