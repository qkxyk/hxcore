using HXCloud.Model;
using HXCloud.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HXCloud.Service
{
    public interface IPlcSecurityService
    {
        /// <summary>
        /// 添加plc鉴权码，返回鉴权码
        /// </summary>
        /// <param name="Account">操作人</param>
        /// <param name="req">鉴权码数据</param>
        /// <returns>返回鉴权码</returns>
        Task<BaseResponse> AddPlcSecurityAsync(string Account, PlcSecurityAddDto req);
    }
}
