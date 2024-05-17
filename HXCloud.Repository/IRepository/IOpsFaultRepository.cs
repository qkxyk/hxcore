using HXCloud.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HXCloud.Repository
{
    public interface IOpsFaultRepository:IBaseRepository<OpsFaultModel>
    {
        /// <summary>
        /// 根据故障码获取故障数据
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        Task<OpsFaultModel> GetOpsFaultByCode(string code);
    }
}
