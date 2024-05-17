using HXCloud.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HXCloud.Repository
{
    public class OpsFaultRepository : BaseRepository<OpsFaultModel>, IOpsFaultRepository
    {
        public override async Task RemoveAsync(OpsFaultModel entity)
        {
            var repairs = await _db.Repairs.Where(a => a.FaultCode == entity.Code).ToListAsync();
            foreach (var item in repairs)
            {
                item.FaultCode = null;
            }
            _db.OpsFaults.Remove(entity);
            await _db.SaveChangesAsync();
        }

        /// <summary>
        /// 根据故障码获取故障数据
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<OpsFaultModel> GetOpsFaultByCode(string code)
        {
            var data = await _db.OpsFaults.Include(a => a.OpsFaultType).Where(a => a.Code == code).FirstOrDefaultAsync();
            return data;
        }
    }
}
