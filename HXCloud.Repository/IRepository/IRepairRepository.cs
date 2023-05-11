using HXCloud.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HXCloud.Repository
{
    public interface IRepairRepository : IBaseRepository<RepairModel>
    {
        Task AddAsync(RepairModel entity, RepairDataModel data);
        Task<RepairModel> GetWithRepairDataAsync(string RepairId);
        IQueryable<RepairModel> GetWithRepairData(Expression<Func<RepairModel, bool>> lambda);
    }
}
