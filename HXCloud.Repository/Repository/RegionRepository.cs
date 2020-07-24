using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using HXCloud.Model;

namespace HXCloud.Repository
{
    public class RegionRepository : BaseRepository<RegionModel>, IRegionRepository
    {
        //添加时更新父节点的删除节点
        public async Task AddAsync(RegionModel entity,RegionModel parent)
        {

        }

        //删除时更新父节点的删除节点
        public  async Task RemoveAsync(RegionModel entity,RegionModel parent)
        {

        }
    }
}
