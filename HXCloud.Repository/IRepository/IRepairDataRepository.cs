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
    }
}
