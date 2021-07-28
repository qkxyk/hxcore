using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HXCloud.Model;
using HXCloud.ViewModel;

namespace HXCloud.Service
{
    public interface ITypeGltfService:IBaseService<TypeGltfModel>
    {
        Task<BaseResponse> GetTypeGltfAsync(int typeId);
        Task<BaseResponse> AddTypeGltfAsync(int typeId, TypeGltfAddDto req, string account, string path);
        Task<BaseResponse> DeleteTypeGltfAsync(int Id, string account, string path);
    }
}
