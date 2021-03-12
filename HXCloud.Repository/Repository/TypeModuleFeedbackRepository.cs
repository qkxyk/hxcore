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
    public class TypeModuleFeedbackRepository : BaseRepository<TypeModuleFeedbackModel>, ITypeModuleFeedbackRepository
    {
        public async Task<IEnumerable<TypeModuleFeedbackModel>> FindWithDataDefineAsync(Expression<Func<TypeModuleFeedbackModel, bool>> predicate)
        {
            var data = await _db.TypeModuleFeedbacks.Include(a => a.TypeDataDefine).Where(predicate).ToListAsync();
            return data;
        }
    }
}
