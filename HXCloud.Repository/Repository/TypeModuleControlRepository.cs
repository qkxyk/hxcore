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
    public class TypeModuleControlRepository : BaseRepository<TypeModuleControlModel>, ITypeModuleControlRepository
    {
        public async Task<TypeModuleControlModel> FindWithFeedbackAsync(int Id)
        {
            var data = await _db.TypeModuleControls.Include(a => a.TypeModuleFeedbacks).FirstOrDefaultAsync(a => a.Id == Id);
            return data;
        }
        public async Task<IEnumerable<TypeModuleControlModel>> FindWithFeedbackAndDataDefineAsync(Expression<Func<TypeModuleControlModel, bool>> predicate)
        {
            var data = await _db.TypeModuleControls.Include(a => a.TypeModuleFeedbacks).ThenInclude(a => a.TypeDataDefine)
                .Where(predicate).ToListAsync();
            return data;
        }
    }
}
