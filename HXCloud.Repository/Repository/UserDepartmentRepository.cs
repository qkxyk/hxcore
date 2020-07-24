using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using HXCloud.Model;
using Microsoft.EntityFrameworkCore;

namespace HXCloud.Repository.Repository
{
    public class UserDepartmentRepository : BaseRepository<UserDepartmentModel>, IUserDepartmentRepository
    {

        public IEnumerable<UserDepartmentModel> FindWithDepartment(Expression<Func<UserDepartmentModel, bool>> lambda)
        {
            var rm = _db.UserDepartments.Include(a => a.Department).Where(lambda).ToList();
            return rm;
        }
    }
}
