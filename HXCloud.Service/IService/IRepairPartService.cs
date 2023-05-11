using HXCloud.Model;
using HXCloud.ViewModel;
using System.Threading.Tasks;

namespace HXCloud.Service
{
    public interface IRepairPartService:IBaseService<RepairPartModel>
    {
        Task<BaseResponse> AddRepairPartAsync(string Account, string Operate, RepairPartAddDto req);
    }
}