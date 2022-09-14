using HXCloud.Model;
using HXCloud.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HXCloud.Service
{
    public interface IBoxService:IBaseService<BoxModel>
    {
        Task<BaseResponse> AddBoxAsync(string account, BoxAddDto req);
        Task<BaseResponse> DeleteBoxAsync(string account, int Id);
        Task<BaseResponse> GetBoxAsync(int Id);
        Task<BaseResponse> GetPageBoxAsync(BasePageRequest req);
        Task<BaseResponse> EncryptDataAsync(string uuid, string serial, string imei);
    }
}
