using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HXCloud.Model;
using HXCloud.ViewModel;

namespace HXCloud.Service
{
    public interface IProjectService : IBaseService<ProjectModel>
    {
        Task<string> GetGroupIdAsync(int projectId);
        Task<BaseResponse> AddProjectAsync(ProjectAddDto req, string account, string GroupId);
        Task<BaseResponse> UpdateProjectAsync(ProjectUpdateDto req, string account);
        Task<BaseResponse> DeleteProjectAsync(string account, int Id);
        Task<BaseResponse> GetProjectWithChildAsync(int Id);
        Task<ProjectModel> GetProjectAsync(int Id);
        Task<BaseResponse> GetGroupProject(string GroupId);
        bool IsExist(int Id, out string pathId, out string groupId);
        Task<string> GetPathId(int Id);
        Task<ProjectCheckDto> GetProjectCheckAsync(int Id);
        Task<BaseResponse> GetMyProjectAsync(string GroupId, string roles, bool isAdmin, BasePageRequest req);
        Task<BaseResponse> GetMySiteAsync(string GroupId, string roles, bool isAdmin, BasePageRequest req);
        Task<List<int>> GetMyProjectIdSync(string GroupId, string roles, bool isAdmin);
        Task<List<int>> GetMySitesIdAsync(string GroupId, string roles, bool isAdmin);
        Task<List<int>> GetProjectSitesIdAsync(int projectId);
        Task<BaseResponse> GetProjectByIdAsync(int Id);
        Task<BaseResponse> GetChildProjectByIdAsync(int Id, ProjectPageRequest req);
    }
}
