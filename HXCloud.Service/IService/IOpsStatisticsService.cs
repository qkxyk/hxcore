using HXCloud.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HXCloud.Service
{
    public interface IOpsStatisticsService
    {
        Task<BaseResponse> GetOpsStatisticsAsync(List<string> users, OpsStatisticsRequest req);
    }
}
