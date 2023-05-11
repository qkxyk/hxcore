using HXCloud.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HXCloud.Repository
{
    public class RepairPartRepository:BaseRepository<RepairPartModel>,IRepairPartRepository
    {
        /// <summary>
        /// 添加运维配件信息，修改运维单是否需要申请配件
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async override Task AddAsync(RepairPartModel entity)
        {
            var rep = await _db.Repairs.FindAsync(entity.RepairId);
            rep.IsParts = true;
            _db.RepairParts.Add(entity);                 
            await _db.SaveChangesAsync();
        }
    }
}
