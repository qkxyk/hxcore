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
    public class TypeImageRepository : BaseRepository<TypeImageModel>, ITypeImageRepository
    {
        public async Task<TypeImageModel> GetTypeImageWithTypeAsync(Expression<Func<TypeImageModel, bool>> predicate)
        {
            var data = await _db.TypeImages.Include(a => a.Type).Where(predicate).FirstOrDefaultAsync();
            return data;
        }
    }
}
