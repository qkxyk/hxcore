using HXCloud.Model;
using HXCloud.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HXCloud.Service
{
    public interface IOpsFaultService : IBaseService<OpsFaultModel>
    {
        /// <summary>
        /// 添加运维故障数据
        /// </summary>
        /// <param name="accout">操作人</param>
        /// <param name="req">故障数据</param>
        /// <returns></returns>
        Task<BaseResponse> AddOpsFaultAsync(string accout, OpsFaultAddDto req);
        /// <summary>
        /// 修改故障数据
        /// </summary>
        /// <param name="account">操作人</param>
        /// <param name="req">故障数据</param>
        /// <returns></returns>
        Task<BaseResponse> EditOpsFaultAsync(string account, OpsFaultEditDto req);
        /// <summary>
        /// 删除故障数据
        /// </summary>
        /// <param name="accout">操作人</param>
        /// <param name="code">故障Code</param>
        /// <returns></returns>
        Task<BaseResponse> DeleteOpsFaultAsync(string accout, string code);
        /// <summary>
        /// 根据故障码获取故障数据
        /// </summary>
        /// <param name="code">故障码</param>
        /// <returns></returns>
        Task<BaseResponse> GetOpsFaultByCodeAsync(string code);
    }
}
