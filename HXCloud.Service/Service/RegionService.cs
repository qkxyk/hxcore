using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using HXCloud.Model;
using HXCloud.Repository;
using HXCloud.ViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HXCloud.Service
{
    public class RegionService : IRegionService
    {
        private readonly ILogger<RegionService> _log;
        private readonly IRegionRepository _rr;

        public RegionService(ILogger<RegionService> log, IRegionRepository rr)
        {
            this._log = log;
            this._rr = rr;
        }
        public async Task<bool> IsExist(Expression<Func<RegionModel, bool>> predicate)
        {
            var data = await _rr.Find(predicate).FirstOrDefaultAsync();
            if (data == null)
            {
                return false;
            }
            return true;
        }

        public async Task<BaseResponse> AddRegionAsync(string account,string groupId,RegionAddDto req)
        {
            //统一组织的,统一层级区域名称不能重复
            var data = await _rr.Find(a => a.GroupId == groupId && a.ParentId == req.ParentId && a.Name == req.Name).FirstOrDefaultAsync();
            if (data!=null)
            {
                return new BaseResponse { Success = false, Message = "该区域下已存在相同的区域名称" };
            }
            try
            {
                string regionId="101";
                //生成区域标示
                if (req.ParentId!=null||req.ParentId.Trim()!="")     //有父区域标示
                {
                    //获取同一个父标示下最后一个添加的区域
                    data =await _rr.Find(a => a.GroupId == groupId && a.ParentId == req.ParentId).OrderByDescending(a => a.Id).FirstOrDefaultAsync();
                    if (data==null)//该父区域下没有节点
                    {
                        regionId = req.ParentId + "001";
                    }
                    else
                    {
                        //有删除的节点回收使用
                        if (data.DeleteId==null||""==data.DeleteId)
                        {

                        }
                    }
                }
                else
                {

                }

            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
