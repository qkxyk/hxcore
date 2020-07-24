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
    public class TypeUpdateFileRepository : BaseRepository<TypeUpdateFileModel>, ITypeUpdateFileRepository
    {
        public async Task<TypeUpdateFileModel> GetTypeUpdateFileWithTypeAsync(Expression<Func<TypeUpdateFileModel, bool>> predicate)
        {
            var data = await _db.TypeUpdateFiles.Include(a => a.Type).Where(predicate).FirstOrDefaultAsync();
            return data;
        }
    }
}
