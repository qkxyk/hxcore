using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HXCloud.Model;
using HXCloud.ViewModel;

namespace HXCloud.Service
{
    public interface IDataDefineWarnCodeService : IBaseService<DataDefineWarnCodeModel>
    {
        Task<DataDefineWarnCodeCheckDto> CheckDataDefineWarnCodeAsync(DataDefineWarnCodeAddDto req);
        Task<BaseResponse> AddDataDefineWarnCodeAsync(string account, DataDefineWarnCodeAddDto req);
        Task<BaseResponse> RemoveDataDefineWarnCodeAsync(string account, int Id);
        Task<BaseResponse> GetDataDefineWarnCodesAsync(bool flag, string[] data);
        Task<BaseResponse> GetPageDataDefineWarnCodesAsync(DataDefineWarnCodePageRequest req);
    }
}
