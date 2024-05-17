using HXCloud.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HXCloud.Repository
{
    public class RepairDataRepository : BaseRepository<RepairDataModel>, IRepairDataRepository
    {
        public async Task AddAsync(RepairDataModel repairData, RepairStatus status)
        {
            var entity = await _db.Repairs.FindAsync(repairData.RepairId);
            entity.RepairStatus = status;
            //如果是成功审核，需要添加完成时间并更新运维单状态
            if (status == RepairStatus.Complete)
            {
                entity.CompleteTime = DateTime.Now;
                entity.IsComplete = true;
            }
            _db.RepairDatas.Add(repairData);
            _db.SaveChanges();
        }
        /// <summary>
        /// 提交运维资料
        /// </summary>
        /// <param name="repairData"></param>
        /// <param name="status"></param>
        /// <param name="faultCode">运维故障码</param>
        /// <returns></returns>
        public async Task AddUploadAsync(RepairDataModel repairData, RepairStatus status, string faultCode)
        {
            var entity = await _db.Repairs.FindAsync(repairData.RepairId);
            entity.RepairStatus = status;
            entity.FaultCode = faultCode;
            _db.RepairDatas.Add(repairData);
            _db.SaveChanges();
        }

    }
}
