using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HXCloud.Model;
using HXCloud.ViewModel;

namespace HXCloud.Service
{
    public interface IProjectImageService : IBaseService<ProjectImageModel>
    {
        Task<string> GetProjectGroupIdAsync(int Id);
        Task<BaseResponse> AddProjectImageAsync(ProjectImageAddDto req, string url, string account, int projectId);
        Task<BaseResponse> RemoveProjectImageAsync(int Id, string account, string path);
        Task<BaseResponse> GetImageAsync(int Id);
        Task<BaseResponse> GetProjectImageAsync(int pId);
    }
}
