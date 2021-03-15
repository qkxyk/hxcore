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
        //验证模块包含的控制项以及配置数据
        public async Task<TypeModuleModel> FindWithControlAsync(int Id)
        {
            var data = await _db.TypeModules.Include(a => a.ModuleControls).Include(a => a.ModeleArguments).Where(a => a.Id == Id).FirstOrDefaultAsync();
            return data;
        }
        public async Task<IEnumerable<TypeModuleModel>> FindWithControlAndFeedbackAsync(Expression<Func<TypeModuleModel, bool>> predicate)
        {
            var data = await _db.TypeModules.AsNoTracking().Include(a => a.ModuleControls).ThenInclude(a => a.TypeClass).Include(a => a.ModuleControls).ThenInclude(a => a.TypeDataDefine).
              Include(a => a.ModuleControls).ThenInclude(a => a.TypeModuleFeedbacks).ThenInclude(a => a.TypeDataDefine)
              .Include(a=>a.ModeleArguments).ThenInclude(a=>a.TypeDataDefine)
              .Where(predicate).ToListAsync();
            return data;
        }

        /// <summary>
        /// 查找模块关联的类型数据
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public async Task<TypeModuleModel> FindWithTypeAsync(Expression<Func<TypeModuleModel, bool>> predicate)
        {
            var data = await _db.TypeModules.Include(a => a.Type).AsNoTracking().Where(predicate).FirstOrDefaultAsync();
            return data;
        }
    }
}
