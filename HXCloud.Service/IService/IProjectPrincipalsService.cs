using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HXCloud.Model;
using HXCloud.ViewModel;

namespace HXCloud.Service
{
    public interface IProjectPrincipalsService : IBaseService<ProjectPrincipalsModel>
    {
        Task<BaseResponse> AddProjectPrincipalsAsync(string account, int projectId, ProjectPrincipalsAddDto req);
        Task<BaseResponse> UpdateProjectPrincipalsAsync(string account, int Id, ProjectPrincipalsUpdateDto req);
        Task<BaseResponse> RemoveProjectPrincipalsAsync(int Id, string account);
        Task<BaseResponse> GetPrincipalAsync(int Id);
        Task<BaseResponse> GetProjectPrincipalsAsync(int pId);
    }
}
