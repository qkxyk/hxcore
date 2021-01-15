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
    public class TypeModuleRepository : BaseRepository<TypeModuleModel>, ITypeModuleRepository
    {
        public async Task<TypeModuleModel> FindWithControlAsync(int Id)
        {
            var data = await _db.TypeModules.Include(a => a.ModuleControls).ThenInclude(a=>a.TypeClass).Where(a => a.Id == Id).FirstOrDefaultAsync();
            return data;
        }
        public async Task<IEnumerable<TypeModuleModel>> FindWithControlAndFeedbackAsync(Expression<Func<TypeModuleModel, bool>> predicate)
        {
            var data = await _db.TypeModules.Include(a=>a.ModuleControls).ThenInclude(a=>a.TypeClass).Include(a => a.ModuleControls).ThenInclude(a=>a.TypeDataDefine).
                ThenInclude(a => a.TypeModuleFeedbacks).ThenInclude(a => a.TypeDataDefine).Where(predicate).ToListAsync();
            return data;
        }
    }
}
