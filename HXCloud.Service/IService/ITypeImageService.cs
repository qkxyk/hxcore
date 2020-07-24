using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HXCloud.Model;
using HXCloud.ViewModel;

namespace HXCloud.Service
{
    public interface ITypeImageService : IBaseService<TypeImageModel>
    {
        Task<BaseResponse> GetTypeImage(int typeId);
        Task<BaseResponse> AddTypeImage(int typeId, TypeImageAddViewModel req, string account, string path);
        Task<BaseResponse> UpdateTypeImage(TypeImageUpdateViewModel req, string account);
        Task<BaseResponse> DeleteTypeImage(int Id, string account, string path);
        Task<string> GetTypeGroupIdAsync(int id);
    }
}
