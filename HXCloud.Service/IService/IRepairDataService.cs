using HXCloud.Model;
using HXCloud.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HXCloud.Service
{
    public interface IRepairDataService : IBaseService<RepairDataModel>
    {
        /// <summary>
        /// 接单
        /// </summary>
        /// <param name="Operate">操作人姓名</param>
        /// <param name="account">操作人账号</param>
        /// <param name="req">单号信息</param>
        /// <returns></returns>
        Task<BaseResponse> ReceiveAsync(string Operate, string account, AddRepairDataBaseDto req);
        /// <summary>
        /// 设置运维单第三方维修
        /// </summary>
        /// <param name="Operate">操作人姓名</param>
        /// <param name="account">操作人账号</param>
        /// <param name="req">设置数据</param>
        /// <returns></returns>
        Task<BaseResponse> ThirdPartAsync(string Operate, string account, AddRepairDataMessageDto req);
        /// <summary>
        /// 设置运维单等待配件
        /// </summary>
        /// <param name="Operate">操作人姓名</param>
        /// <param name="account">操作人账号</param>
        /// <param name="req">设置数据</param>
        /// <returns></returns>
        Task<BaseResponse> WaitAsync(string Operate, string account, AddRepairDataMessageDto req);
        /// <summary>
        /// 上传运维凭证数据
        /// </summary>
        /// <param name="Operate">操作人姓名</param>
        /// <param name="account">操作人账号</param>
        /// <param name="req">凭证数据</param>
        /// <returns></returns>
        Task<BaseResponse> UploadAsync(string Operate, string account, RepairSubmitDto req);
        /// <summary>
        /// 审核运维流程单
        /// </summary>
        /// <param name="Operate">操作人姓名</param>
        /// <param name="account">操作人账号</param>
        /// <param name="complete">流程是否结束</param>
        /// <param name="req">审核数据</param>
        /// <returns></returns>
        Task<BaseResponse> CheckAsync(string Operate, string account, bool complete, AddRepairCheckDto req);
    }
}
