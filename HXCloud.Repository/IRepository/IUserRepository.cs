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
    }
}
