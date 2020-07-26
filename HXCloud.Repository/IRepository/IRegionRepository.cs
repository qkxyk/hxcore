using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HXCloud.Model;

namespace HXCloud.Repository
{
    public interface IRegionRepository:IBaseRepository<RegionModel>
    {
        Task AddAsync(RegionModel entity, RegionModel parent, bool isModify);
        Task RemoveAsync(RegionModel entity, RegionModel parent);
    }
}
