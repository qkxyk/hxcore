using HXCloud.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HXCloud.Repository
{
    public class RepairRepository : BaseRepository<RepairModel>, IRepairRepository
    {
        //public async override Task AddAsync(RepairModel entity)
        //{
        //    _db.Repairs.Add(entity);
        //    //更改调试单或者维修单关联的问题单状态
        //    if (entity.IssueId != 0)
        //    {
        //        var issue = await _db.FindAsync<IssueModel>(entity.IssueId);
        //        if (issue != null)
        //        {
        //            issue.Status = true;
        //        }
        //    }
        //    //添加派单流程

        //    await _db.SaveChangesAsync();
        //}
        public async Task AddAsync(RepairModel entity, RepairDataModel data)
        {
            _db.Repairs.Add(entity);
            //更改调试单或者维修单关联的问题单状态
            if (entity.IssueId != 0)
            {
                var issue = await _db.FindAsync<IssueModel>(entity.IssueId);
                if (issue != null)
                {
                    issue.Status = true;
                    issue.Dt = DateTime.Now;
                }
            }
            //data.Repair = entity;
            //添加派单流程
            _db.RepairDatas.Add(data);
            await _db.SaveChangesAsync();
        }

        public async override Task RemoveAsync(RepairModel entity)
        {
            _db.Repairs.Remove(entity);
            //更改调试单或者维修单关联的问题单状态
            if (entity.IssueId != 0)
            {
                var issue = await _db.FindAsync<IssueModel>(entity.IssueId);
                if (issue != null)
                {
                    issue.Status = false;
                }
            }
            //删除派单流程
            var data = await _db.RepairDatas.Where(a => a.RepairId == entity.Id).ToListAsync();
            _db.RepairDatas.RemoveRange(data);
            await _db.SaveChangesAsync();
        }

        public async Task<RepairModel> GetWithRepairDataAsync(string RepairId)
        {
            var data = await _db.Repairs.Include(a => a.RepairDatas).FirstOrDefaultAsync(a => a.Id == RepairId);
            return data;
        }
        public IQueryable<RepairModel> GetWithRepairData(Expression<Func<RepairModel, bool>> lambda)
        {
            var ret = _db.Set<RepairModel>()/*.Include(a => a.Issue)*/.Include(a => a.RepairDatas).AsNoTracking().Where(lambda);
            return ret;
        }
    }
}
