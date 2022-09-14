using HXCloud.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace HXCloud.Repository
{
    public class PatrolDataRepository : BaseRepository<PatrolDataModel>, IPatrolDataRepository
    {
        public IQueryable<PatrolDataModel> FindWithPatrolData(Expression<Func<PatrolDataModel, bool>> lambda)
        {
            //var data = _db.Devices.Include(a => a.DeviceOnline).Include(a => a.DeviceImage).Where(predicate);
            var ret = _db.Set<PatrolDataModel>().Include(a => a.PatrolImage).Include(a => a.ProductData).Include(a => a.WaterAnalysis).Include(a => a.DevicePatrol).Include(a => a.TechniquePatrol)
                .Where(lambda);
            return ret;
        }
    }
}
