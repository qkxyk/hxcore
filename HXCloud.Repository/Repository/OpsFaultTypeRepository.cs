using HXCloud.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HXCloud.Repository
{
    public class OpsFaultTypeRepository : BaseRepository<OpsFaultTypeModel>, IOpsFaultTypeRepository
    {

        /// <summary>
        /// 顶级节点
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<OpsFaultTypeModel> FindWithOpsFaultAsync(int Id)
        {
            var data = await _db.OpsFaultTypes.Include(a => a.Child).ThenInclude(a => a.OpsFalt)
                .FirstOrDefaultAsync(a => a.FaultTypeId == Id);
            return data;
        }
        /// <summary>
        /// 叶子节点
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<OpsFaultTypeModel> FindWithOpsFaultChildAsync(int Id)
        {
            var data = await _db.OpsFaultTypes.Include(a => a.OpsFalt)
                         .FirstOrDefaultAsync(a => a.FaultTypeId == Id);
            return data;
        }
        public async Task<List<OpsFaultTypeModel>> FindAllWithOpsFaultAsync()
        {
            var data = await _db.OpsFaultTypes.Include(a => a.Child).ThenInclude(a => a.OpsFalt).Where(a=>a.Parent==null).ToListAsync();
            return data;
        }
        public async Task<List<OpsFaultTypeModel>> FindByParentIdWithOpsFaultAsync(int Id)
        {
            var data = await _db.OpsFaultTypes.Include(a => a.Child).ThenInclude(a => a.OpsFalt)
                .Where(a => a.ParentId == Id).ToListAsync();
            return data;
        }
    }
}
