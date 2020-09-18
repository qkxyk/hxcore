using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HXCloud.Model;
using HXCloud.ViewModel;

namespace HXCloud.Service
{
    public interface IDataDefineLibraryService : IBaseService<DataDefineLibraryModel>
    {
        Task<BaseResponse> AddDataDefineAsync(DataDefineLibraryAddDto req, string account);
        Task<BaseResponse> UpdateDataDefineAsync(DataDefineLibraryUpdateDto req, string account);
        Task<BaseResponse> DeleteDataDefineAsync(int Id, string account);
        Task<BaseResponse> GetDataDefineLibraryAsync(int Id);
        Task<BaseResponse> GetDataDefineLibrarysAsync(DataDefineLibraryPageRequest req);
    }
}
