using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HXCloud.Model;
using HXCloud.ViewModel;

namespace HXCloud.Service
{
    public interface ITypeModuleFeedbackService : IBaseService<TypeModuleFeedbackModel>
    {
        /// <summary>
        /// 添加控制项反馈数据
        /// </summary>
        /// <param name="Account">操作人</param>
        /// <param name="req">反馈数据</param>
        /// <returns></returns>
        Task<BaseResponse> AddTypeModuleFeedbaskAsync(string Account, TypeModuleFeedbackAddDto req);
        /// <summary>
        /// 修改控制项反馈数据
        /// </summary>
        /// <param name="Account">操作人</param>
        /// <param name="req">反馈数据</param>
        /// <returns></returns>
        Task<BaseResponse> UpdateTypeModuleFeedbackAsync(string Account, TypeModuleFeedbackUpdateDto req);
        /// <summary>
        /// 删除控制项反馈数据
        /// </summary>
        /// <param name="Account">操作人</param>
        /// <param name="Id">反馈项标示</param>
        /// <returns></returns>
        Task<BaseResponse> DeleteTypeModuleFeedbackAsync(string Account, int Id);

        /// <summary>
        /// 获取控制项的反馈数据
        /// </summary>
        /// <param name="ControlId">控制项标示</param>
        /// <returns></returns>
        Task<BaseResponse> GetFeedbackByControlIdAsync(int ControlId);
    }
}
