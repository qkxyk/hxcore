using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HXCloud.Model;
using HXCloud.ViewModel;

namespace HXCloud.Service
{
    public interface ITypeUpdateFileService : IBaseService<TypeUpdateFileModel>
    {
        Task<BaseResponse> AddTypeUpdateFile(int typeId, TypeUpdateFileAddViewModel req, string account, string url);
        Task<BaseResponse> UpdateTypeUpdateFile(TypeUpdateFileUpdateViewModel req, string account);
        Task<BaseResponse> GetFile(int Id);
        Task<BaseResponse> GetTypeUpdateFile(int TypeId);
        Task<string> GetTypeGroupIdAsync(int FileId);
        Task<BaseResponse> DeleteUpdateFile(int Id, string account, string path);
    }
}
