using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HXCloud.Model;

namespace HXCloud.Repository
{
    public interface IProjectImageRepository : IBaseRepository<ProjectImageModel>
    {
        Task<ProjectImageModel> GetWithProject(int Id);

    }
}
