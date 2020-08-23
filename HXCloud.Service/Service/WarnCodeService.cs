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
    public class WarnCodeService : IWarnCodeService
    {
        private readonly ILogger _log;
        private readonly IMapper _mapper;
        private readonly IWarnCodeRepository _warnCode;

        public WarnCodeService(ILogger log,IMapper mapper,IWarnCodeRepository warnCode)
        {
            this._log = log;
            this._mapper = mapper;
            this._warnCode = warnCode;
        }
        public Task<bool> IsExist(Expression<Func<WarnCodeModel, bool>> predicate)
        {
            throw new NotImplementedException();
        }
    }
}
