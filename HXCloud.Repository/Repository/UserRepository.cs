using HXCloud.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HXCloud.Repository
{
    public class UserRepository : BaseRepository<UserModel>, IUserRepository
    {
        //public UserRepository(HXCloudContext context):base(context)
        //{

        //}
        public async Task<UserModel> FindWithGroup(Expression<Func<UserModel, bool>> predicate)
        {
            var rm = await _db.Users.Include(a => a.Group)/*.Include(a => a.UserRoles)*/.Where(predicate).FirstOrDefaultAsync();
            return rm;
        }
    }
}
