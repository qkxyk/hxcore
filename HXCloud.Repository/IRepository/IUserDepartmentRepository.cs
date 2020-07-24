using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using HXCloud.Model;

namespace HXCloud.Repository
{
    public interface IUserDepartmentRepository : IBaseRepository<UserDepartmentModel>
    {
        IEnumerable<UserDepartmentModel> FindWithDepartment(Expression<Func<UserDepartmentModel, bool>> lambda);
    }
}
