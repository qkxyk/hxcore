using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using HXCloud.Model;
using Microsoft.EntityFrameworkCore;

namespace HXCloud.Repository
{
    public class TypeSchemaRepository : BaseRepository<TypeSchemaModel>, ITypeSchemaRepository
    {
        public async Task<TypeSchemaModel> FindWithType(Expression<Func<TypeSchemaModel, bool>> lambda)
        {
            var data = await _db.TypeSchemas.Include(a => a.Type).Where(lambda).FirstOrDefaultAsync();
            return data;
        }
        public async Task<TypeSchemaModel> FindWithChild(int Id)
        {
            var data = await _db.TypeSchemas.Include(a => a.Child).Where(a => a.Id == Id).FirstOrDefaultAsync();
            return data;
        }
    }
}
