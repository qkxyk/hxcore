using HXCloud.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HXCloud.Repository
{
    public interface IRepairDataRepository : IBaseRepository<RepairDataModel>
    {
        Task AddAsync(RepairDataModel repairData, RepairStatus status);
        /// <summary>
        /// 提交运维资料
        /// </summary>
        /// <param name="repairData"></param>
        /// <param name="status"></param>
        /// <param name="faultCode">运维故障码</param>
        /// <returns></returns>
        Task AddUploadAsync(RepairDataModel repairData, RepairStatus status, string faultCode);
    }
}
