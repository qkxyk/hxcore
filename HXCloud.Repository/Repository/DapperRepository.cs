using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Microsoft.Extensions.Configuration;
using MySqlConnector;

namespace HXCloud.Repository
{
    /// <summary>
    /// dapper依赖注入配置文件
    /// </summary>
    public class DapperRepository
    {
        private readonly IConfiguration _config;
        public DapperRepository(IConfiguration config)
        {
            _config = config;
        }
        public IDbConnection Connection => new MySqlConnection(_config.GetConnectionString("Mysql"));
    }
}
