using HXCloud.Model;
using HXCloud.Repository;
using HXCloud.ViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HXCloud.Service
{
    public class OpsStatisticsService : IOpsStatisticsService
    {
        private readonly IRepairRepository _repair;

        public OpsStatisticsService(IRepairRepository repair)
        {
            this._repair = repair;
        }

        /// <summary>
        ///获取运维统计数据，完成的只统计本月数据，未完成的统计全部 
        /// </summary>
        /// <param name="req">查询条件</param>
        /// <param name="isAdmin">是否管理员</param>
        /// <param name="account">非管理员没有查询权限的查找自己</param>
        /// <param name="DeviceSn">非管理员有查询权限查看的设备列表</param>
        /// <returns></returns>
        public async Task<BaseResponse> GetOpsStatisticsAsync( OpsStatisticsRequest req, bool isAdmin, string account, List<string> DeviceSn)
        {
            var begin = GetMonthStart();
            var end = GetMonthEnd();
            var rep = _repair.Find(a => 1==1);
          
            if (!isAdmin)
            {
                if (account != null)//查询自己的
                {
                    rep = rep.Where(a => a.Receiver == account);
                }
                else//有查询权限的角色查看有权限设备的数据
                {
                    rep = rep.Where(a => DeviceSn.Contains(a.DeviceSn));
                }
            }
            var RepCom = rep.Where(a => a.CompleteTime >= begin && a.CompleteTime <= end);//本月完成
            var repSend = rep.Where(a => a.CreateTime >= begin && a.CreateTime <= end);//本月派的单
            //维修单
            int Rtotal = await repSend.CountAsync(a => a.RepairType == RepairType.Repair);//全部派单情况
            int Rcomplete = await RepCom.CountAsync(a => a.RepairType == RepairType.Repair && a.IsComplete == true);//完成的数量
            int RunCompleted = await rep.CountAsync(a => a.RepairType == RepairType.Repair && a.IsComplete == false);//未完成的数量

            //调试单
            int total = await repSend.CountAsync(a => a.RepairType == RepairType.Debug);//全部派单情况
            int complete = await RepCom.CountAsync(a => a.RepairType == RepairType.Debug && a.IsComplete == true);//完成的数量
            int unCompleted = await rep.CountAsync(a => a.RepairType == RepairType.Debug && a.IsComplete == false);//未完成的数量

            BResponse<OpsStatisticsResponseDto> dto = new BResponse<OpsStatisticsResponseDto>();
            dto.Data = new OpsStatisticsResponseDto();
            dto.Data.Repair = new RepairStatisticsDto { Total = Rtotal, Complete = Rcomplete, InComplete = RunCompleted };
            dto.Data.Debug = new RepairStatisticsDto { Total = total, Complete = complete, InComplete = unCompleted };
            dto.Success = true;
            dto.Message = "获取统计数据成功";
            return dto;
        }
        /// <summary>
        /// 获取运维统计数据，完成的只统计本月数据，未完成的统计全部
        /// </summary>
        /// <param name="users">用户列表</param>
        /// <param name="req">查询参数</param>
        /// <returns></returns>
        public async Task<BaseResponse> GetOpsStatisticsAsync(List<string> users, OpsStatisticsRequest req)
        {
            var begin = GetMonthStart();
            var end = GetMonthEnd();
            var rep = _repair.Find(a => users.Contains(a.Receiver));
            var RepCom = rep.Where(a => a.CompleteTime >= begin && a.CompleteTime <= end);//本月完成
            var repSend = rep.Where(a => a.CreateTime >= begin && a.CreateTime <= end);//本月派的单
            //维修单
            int Rtotal = await repSend.CountAsync(a => a.RepairType == RepairType.Repair);//全部派单情况
            int Rcomplete = await RepCom.CountAsync(a => a.RepairType == RepairType.Repair && a.IsComplete == true);//完成的数量
            int RunCompleted = await rep.CountAsync(a => a.RepairType == RepairType.Repair && a.IsComplete == false);//未完成的数量

            //调试单
            int total = await repSend.CountAsync(a => a.RepairType == RepairType.Debug);//全部派单情况
            int complete = await RepCom.CountAsync(a => a.RepairType == RepairType.Debug && a.IsComplete == true);//完成的数量
            int unCompleted = await rep.CountAsync(a => a.RepairType == RepairType.Debug && a.IsComplete == false);//未完成的数量

            BResponse<OpsStatisticsResponseDto> dto = new BResponse<OpsStatisticsResponseDto>();
            dto.Data = new OpsStatisticsResponseDto();
            dto.Data.Repair = new RepairStatisticsDto { Total = Rtotal, Complete = Rcomplete, InComplete = RunCompleted };
            dto.Data.Debug = new RepairStatisticsDto { Total = total, Complete = complete, InComplete = unCompleted };
            dto.Success = true;
            dto.Message = "获取统计数据成功";
            return dto;
        }
        public DateTime GetMonthStart()
        {
            //获取本月的起始日期
            DateTime dt = DateTime.Now;
            //本月第一天时间
            DateTime dt_First = dt.AddDays(-dt.Day + 1);
            dt_First = Convert.ToDateTime(dt_First.ToString("yyyy-MM-dd 00:00:00"));
            return dt_First;
        }
        public DateTime GetMonthEnd()
        {
            DateTime dt = DateTime.Now;
            //获得某年某月的天数
            int year = dt.Date.Year;
            int month = dt.Date.Month;
            int dayCount = DateTime.DaysInMonth(year, month);
            //本月最后一天时间
            var dt_First = GetMonthStart();
            DateTime dt_Last = dt_First.AddDays(dayCount - 1);
            dt_Last = Convert.ToDateTime(dt_Last.ToString("yyyy-MM-dd 23:59:59"));
            return dt_Last;
        }
    }
}
