using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using HXCloud.Model;

namespace HXCloud.Repository
{
    public class RegionRepository : BaseRepository<RegionModel>, IRegionRepository
    {
        //添加时更新父节点的删除节点
        public async Task AddAsync(RegionModel entity, RegionModel parent, bool isModify)
        {
            //_db.Set<RegionModel>..Regions.Add(entity);
            _db.Regions.Add(entity);
            if (parent != null && isModify)
            {
                _db.Entry<RegionModel>(parent).State = EntityState.Modified;//更新父节点
            }
            await _db.SaveChangesAsync();
        }

        //删除时更新父节点的删除节点
        public async Task RemoveAsync(RegionModel entity, RegionModel parent)
        {
            _db.Regions.Remove(entity);
            if (parent != null)
            {
                _db.Entry(parent).State = EntityState.Modified;
            }
            await _db.SaveChangesAsync();
        }
    }
}
