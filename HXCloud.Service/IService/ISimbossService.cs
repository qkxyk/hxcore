using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HXCloud.Model;
using HXCloud.ViewModel;

namespace HXCloud.Service
{
    public interface ISimbossService : IBaseService<SimbossModel>
    {
        /// <summary>
        /// 获取simboss数据。注：数据较少，不做分页
        /// </summary>
        /// <returns></returns>
        Task<BaseResponse> GetSimbossAsync();
        /// <summary>
        /// 添加simboss数据
        /// </summary>
        /// <param name="Account">操作人</param>
        /// <param name="req">simboss数据</param>
        /// <returns></returns>
        Task<BaseResponse> AddSimbossAsync(string Account, SimbossAddDto req);
        /// <summary>
        /// 更新simboss数据
        /// </summary>
        /// <param name="Account">操作人</param>
        /// <param name="req">simboss数据</param>
        /// <returns></returns>
        Task<BaseResponse> UpdateSimbossAsync(string Account, SimbossUpdateDto req);
        /// <summary>
        /// 删除simboss数据
        /// </summary>
        /// <param name="Account">操作人</param>
        /// <param name="Id">要删除的标识</param>
        /// <returns></returns>
        Task<BaseResponse> DeleteSimbossAsync(string Account, int Id);
    }
}
