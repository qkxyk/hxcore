using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using HXCloud.Model;
using HXCloud.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HXCloud.Service
{
    public class RoleProjectService : IRoleProjectService
    {
        private readonly ILogger<RoleProjectService> _log;
        private readonly IRoleProjectRepository _rp;

        public RoleProjectService(ILogger<RoleProjectService> log, IRoleProjectRepository rp)
        {
            this._log = log;
            this._rp = rp;
        }
        public Task<bool> IsExist(Expression<Func<RoleProjectModel, bool>> predicate)
        {
            throw new NotImplementedException();
        }
        //验证角色是否权限(非管理员)
        public async Task<bool> IsAuth(string roles, string pathId, int operate)
        {
            if (pathId.Trim().Count() == 0)  //无项目的设备只有管理员有权限
            {
                return false;
            }
            int[] rs = Array.ConvertAll<string, int>(roles.Split(','), src => int.Parse(src));
            var data = await _rp.Find(a => rs.Contains(a.RoleId) && (int)a.Operate >= operate).ToListAsync();
            if (data == null)
            {
                return false;
            }
            int[] ps = Array.ConvertAll(pathId.Split('/'), src => int.Parse(src));
            foreach (var item in data)
            {
                if (ps.Contains(item.ProjectId))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
