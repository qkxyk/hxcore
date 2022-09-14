using HXCloud.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace HXCloud.Repository
{
    public  interface IPatrolDataRepository:IBaseRepository<PatrolDataModel>
    {
        IQueryable<PatrolDataModel> FindWithPatrolData(Expression<Func<PatrolDataModel, bool>> lambda);
    }
}
