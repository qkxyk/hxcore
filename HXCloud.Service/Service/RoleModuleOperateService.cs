using HXCloud.Model;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HXCloud.Service
{
    public class RoleModuleOperateService : IRoleModuleOperateService
    {
        public Task<bool> IsExist(Expression<Func<RoleModuleOperateModel, bool>> predicate)
        {
            throw new NotImplementedException();
        }
    }
}
