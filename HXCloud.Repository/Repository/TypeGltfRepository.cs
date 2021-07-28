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
    public class TypeGltfRepository : BaseRepository<TypeGltfModel>, ITypeGltfRepository
    {
        public async Task<TypeGltfModel> FindAsync(Expression<Func<TypeGltfModel, bool>> predicate)
        {
            var data = await _db.TypeGltfs.Where(predicate).FirstOrDefaultAsync();
            return data;
        }
    }
}
