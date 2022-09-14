using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using HXCloud.Model;
using Microsoft.EntityFrameworkCore;

namespace HXCloud.Repository
{
    public class ProjectRepository : BaseRepository<ProjectModel>, IProjectRepository
    {
        public async Task<ProjectModel> FindWithChildAsync(Expression<Func<ProjectModel, bool>> predicate)
        {
            var data = await _db.Projects.Include(a => a.Child).Where(predicate).FirstOrDefaultAsync();
            return data;
        }
        public IQueryable<ProjectModel> FindWithImageAndChildAsync(Expression<Func<ProjectModel, bool>> predicate)
        {
            var data = _db.Projects.Include(a => a.Child).Include(a => a.Images).Where(predicate);//.FirstOrDefaultAsync();
            return data;
        }
        public IQueryable<ProjectModel> FindProjectsWithImageByParentAsync(Expression<Func<ProjectModel, bool>> predicate)
        {
            var data = _db.Projects.Include(a => a.Child).Include(a => a.Images).Where(predicate);//.FirstOrDefaultAsync();
            return data;
        }

        public IQueryable<ProjectModel> FindWithImageAndDeviceAsync(Expression<Func<ProjectModel, bool>> predicate)
        {
            var data = _db.Projects.Include(a => a.Images).Include(a => a.Devices).Where(predicate);//.FirstOrDefaultAsync();
            return data;
        }

        public async Task<Dictionary<string, DeviceCardInfoModel>> FindWithDeviceCardsAsync(List<int> sitesId)
        {
            var data = from device in _db.Devices
                       join cards in _db.DeviceCards on device.DeviceSn equals cards.DeviceSn
                       where cards.ICCID != null && sitesId.Contains(device.ProjectId.Value)
                       select new DeviceCardInfoModel
                       {
                           FullName = device.FullName,
                           DeviceName = device.DeviceName,
                           DeviceSn = cards.DeviceSn,
                           ICCID = cards.ICCID,
                           ProjectId = device.ProjectId,
                           FullId = device.FullId,
                           DeviceNo = device.DeviceNo
                       };
            var ret = await data.ToListAsync();
            Dictionary<string, DeviceCardInfoModel> dicRet = new Dictionary<string, DeviceCardInfoModel>();
            foreach (var item in ret)
            {
                dicRet.Add(item.ICCID, item);
            }
            return dicRet;
        }
    }
}
