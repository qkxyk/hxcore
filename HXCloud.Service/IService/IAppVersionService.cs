using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HXCloud.Model;
using HXCloud.ViewModel;

namespace HXCloud.Service
{
    public interface IAppVersionService : IBaseService<AppVersionModel>
    {
        Task<BaseResponse> AddAppVersionAsync(string account, string path, AppVersionAddDto req);
        Task<BaseResponse> UpdateAppVersionAsync(string account, AppVersionUpdateDto req);
        Task<BaseResponse> DeleteAppVersionAsync(string account, int Id, string path);
        Task<BaseResponse> GetAppVersionAsync();
        Task<BaseResponse> GetPageAppVersionAsync(BasePageRequest req);
    }
}
