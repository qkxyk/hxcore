using AutoMapper;
using HXCloud.Model;
using HXCloud.Repository;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HXCloud.Service
{
    public class WarnTypeService : IWarnTypeService
    {
        private readonly ILogger _log;
        private readonly IMapper _mapper;
        private readonly IWarnTypeRepository _warnType;

        public WarnTypeService(ILogger log,IMapper mapper,IWarnTypeRepository warnType)
        {
            this._log = log;
            this._mapper = mapper;
            this._warnType = warnType;
        }
        public Task<bool> IsExist(Expression<Func<WarnTypeModel, bool>> predicate)
        {
            throw new NotImplementedException();
        }
    }
}
