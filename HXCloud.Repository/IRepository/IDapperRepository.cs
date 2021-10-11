using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace HXCloud.Repository.IRepository
{
    /// <summary>
    /// 提供dapper连接
    /// </summary>
    public interface IDapperRepository
    {
        public IDbConnection Connection { get; }
    }
}
