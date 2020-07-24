using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HXCloud.Model;

namespace HXCloud.Repository
{
    public interface IGroupRepository : IBaseRepository<GroupModel>
    {
        Task Add(GroupModel entity, UserModel user);
    }
}
