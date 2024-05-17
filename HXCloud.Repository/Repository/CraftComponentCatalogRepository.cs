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
    public class CraftComponentCatalogRepository : BaseRepository<CraftComponentCatalogModle>, ICraftComponentCatalogRepository
    {

        public async Task<List<CraftComponentCatalogModle>> GetAll(Expression<Func<CraftComponentCatalogModle, bool>> predicate)
        {
            var data = await _db.CraftComponentCatelogs.Include(a=>a.CraftElements).Include(a=>a.Child).ThenInclude(a => a.CraftElements).Where(predicate).ToListAsync();//.Where(a => a.CraftElements
            return data;
        }
        public async Task<CraftComponentCatalogModle> GetWithElementAsync(Expression<Func<CraftComponentCatalogModle, bool>> predicate)
        {
            var data = await _db.CraftComponentCatelogs.Include(a => a.Child).Include(a => a.CraftElements).Where(predicate).FirstOrDefaultAsync();
            return data;
        }
        public async Task<List<CraftComponentCatalogModle>> GetUser(Func<CraftElementModle, bool> predicate)
        {
            //var data = _db.CraftComponentCatelogs.Include(a => a.CraftElements).Where(a =>
            //a.CraftElements.Any(d => d.ElementType == ElementType.Personal && d.UserId == userId));
            //await data.ForEachAsync(a => a.CraftElements.Where(a => a.ElementType == ElementType.Personal && a.UserId == userId));
            var data = await _db.CraftComponentCatelogs.Include(a => a.CraftElements).ToListAsync();
            data.ForEach(a =>
            {
                var t = a.CraftElements.Where(predicate).ToList();
                for (int i = 0; i < t.Count; i++)
                {
                    a.CraftElements.Remove(t[i]);
                }
            });
            return data;
        }
    }
}
