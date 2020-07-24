using System.Threading.Tasks;
using HXCloud.Model;
using HXCloud.ViewModel;

namespace HXCloud.Service
{
    public interface IGroupService : IBaseService<GroupModel>
    {
        Task<BaseResponse> AddGroupAsync(GroupAddViewModel req, string account);
        Task<BaseResponse> GetGroupAsync(string Id);
        Task<BaseResponse> GetGroupsAsync(GroupListRequest req);
        Task<BaseResponse> GetPageGroupsAsync(GroupPageListRequest req);
        Task<BaseResponse> UpdateLogoAsync(string groupId, string url, string account);
        Task<BaseResponse> UpdateAsync(GroupUpdateViewModel req, string account);

        Task<string> GetMasterIdAsync(string code);
        //bool IsExist(Expression<Func<GroupModel, bool>> predicate);
    }
}
