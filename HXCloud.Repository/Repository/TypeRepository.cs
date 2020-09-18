using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using HXCloud.Model;
using Microsoft.EntityFrameworkCore;

namespace HXCloud.Repository
{
    public class TypeRepository : BaseRepository<TypeModel>, ITypeRepository
    {
        /// <summary>
        /// 获取类型和类型的子节点
        /// </summary>
        /// <param name="predicate">类型表达式</param>
        /// <returns>返回满足条件的第一个类型</returns>
        public async Task<TypeModel> FindAsync(Expression<Func<TypeModel, bool>> predicate)
        {
            var data = await _db.Types.Include(a => a.Child).Where(predicate).FirstOrDefaultAsync();
            return data;
        }
        public async Task CopyTypeAsync(int sourceId, TypeModel target)
        {
            //获取配件和控制项
            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    #region 处理类型数据定义

                    #endregion
                    #region 处理类型配件
                    target.TypeAccessories = new List<TypeAccessoryModel>();
                    var dc = await _db.TypeAccessories.Include(a => a.TypeAccessoryControlDatas).Where(a => a.TypeId == sourceId).ToListAsync();
                    foreach (var item in dc)
                    {
                        TypeAccessoryModel tam = new TypeAccessoryModel
                        {
                            Type = target,
                            Create = target.Create,
                            ICON = item.ICON,
                            Name = item.Name
                        };
                        tam.TypeAccessoryControlDatas = new List<TypeAccessoryControlDataModel>();
                        foreach (var it in item.TypeAccessoryControlDatas)
                        {
                            TypeAccessoryControlDataModel tacm = new TypeAccessoryControlDataModel
                            {
                                TypeAccessory = tam,
                                AssociateDefineId = it.AssociateDefineId,
                                ControlName = it.ControlName,
                                Create = target.Create,
                                DataDefineId = it.DataDefineId,
                                DataValue = it.DataValue,
                                IState = it.IState,
                                SequenceIn = it.SequenceIn,
                                SequenceOut = it.SequenceOut
                            };
                            tam.TypeAccessoryControlDatas.Add(tacm);
                        }
                        target.TypeAccessories.Add(tam);
                    }
                    #endregion
                    await _db.Types.AddAsync(target);
                    _db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
            }
        }
    }
}
