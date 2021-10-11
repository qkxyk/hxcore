using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using HXCloud.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace HXCloud.Repository
{
    public class DeviceHisDataRepository : DapperRepository, IDeviceHisDataRepository
    {
        //public IQueryable<DeviceHisDataModel> FindWithDevice(Expression<Func<DeviceHisDataModel, bool>> lambda)
        //{
        //    var ret = _db.DeviceHisData.Include(a => a.Device).AsNoTracking().Where(lambda);
        //    return ret;
        //}
        public DeviceHisDataRepository(IConfiguration config) : base(config)
        {

        }

        public async Task<bool> CheckTableExistAsyn(string TableName)
        {
            using (IDbConnection conn = Connection)
            {
                string sql = $"show tables like '{TableName}'";
                var data = await conn.QueryAsync(sql);
                //  var data = await conn.QueryFirstOrDefaultAsync(sql);//.Query(sql).FirstOrDefaultAsync();//.Count();
                if (data.Count() <= 0)
                {
                    return false;
                }
                return true;
            }
        }

        public async Task<IEnumerable<CoreDeviceHisDataMoel>> FindDeviceHisDataAsync(string TableName, string DeviceSn)
        {
            using (var conn = Connection)
            {
                //从mysql中获取数据
                string strSql = $"select * from {TableName} where devicesn=@DeviceSn;";
                var query = await conn.QueryAsync<CoreDeviceHisDataMoel>(strSql, new { DeviceSn = DeviceSn });
                return query.ToList();
            }

        }

        public async Task<CoreDeviceHisDataMoel> FindFirstOrDefaultAsync(string TableName, string DeviceSn,string orderType)
        {
            using (var conn = Connection)
            {
                //从mysql中获取数据
                string strSql = $"select * from {TableName} where devicesn=@DeviceSn order by dt {orderType};";
                var data = await conn.QueryFirstOrDefaultAsync<CoreDeviceHisDataMoel>(strSql, new { DeviceSn = DeviceSn });
                return data;
            }
        }
    }
}
