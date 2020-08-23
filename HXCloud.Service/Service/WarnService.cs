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
    public class WarnService : IWarnService
    {
        private readonly ILogger<WarnService> _log;
        private readonly IMapper _mapper;
        private readonly IWarnRepository _warn;

        public WarnService(ILogger<WarnService> log, IMapper mapper, IWarnRepository warn)
        {
            this._log = log;
            this._mapper = mapper;
            this._warn = warn;
        }
        public Task<bool> IsExist(Expression<Func<WarnModel, bool>> predicate)
        {
            throw new NotImplementedException();
        }
    }
}
