using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HXCloud.Model;
using HXCloud.ViewModel;

namespace HXCloud.Service
{
    public interface ITypeDataDefineService : IBaseService<TypeDataDefineModel>
    {
        Task<string> GetTypeGroupIdAsync(int id);
        Task<BaseResponse> AddTypeDataDefine(int typeId, TypeDataDefineAddViewModel req, string account);
        Task<BaseResponse> TypeDataDefineUpdate(TypeDataDefineUpdateViewModel req, string account);
        Task<BaseResponse> DeleteTypeDataDefine(int Id, string account);
        Task<BaseResponse> GetDataDefine(int Id);
        Task<BaseResponse> GetTypeDataDefines(int typeId, TypeDataDefinePageRequest req);
    }
}
