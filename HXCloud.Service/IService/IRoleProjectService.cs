using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HXCloud.Model;

namespace HXCloud.Service
{
    public interface IRoleProjectService : IBaseService<RoleProjectModel>
    {
        Task<bool> IsAuth(string roles, string pathId, int operate);
    }
}
