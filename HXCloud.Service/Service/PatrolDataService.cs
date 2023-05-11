using AutoMapper;
using HXCloud.Model;
using HXCloud.Repository;
using HXCloud.ViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace HXCloud.Service
{
    public class PatrolDataService : IPatrolDataService
    {
        private readonly ILogger<PatrolDataService> _logger;
        private readonly IMapper _mapper;
        private readonly IPatrolDataRepository _patrolData;
        private readonly IPatrolImageRepository _patrolImage;
        private readonly IProductDataRepository _productData;
        private readonly IWaterAnalysisRepository _waterAnalysis;
        private readonly IDevicePatrolRepository _devicePatrol;
        private readonly ITechniquePatrolRepository _techniquePatrol;
        private readonly IOpsItemRepository _opsItem;
        private readonly ITypeOpsItemRepository _typeOps;

        public PatrolDataService(ILogger<PatrolDataService> logger, IMapper mapper, IPatrolDataRepository patrolData, IPatrolImageRepository patrolImage, IProductDataRepository productData,
            IWaterAnalysisRepository waterAnalysis, IDevicePatrolRepository devicePatrol, ITechniquePatrolRepository techniquePatrol, IOpsItemRepository opsItem, ITypeOpsItemRepository typeOps)
        {
            this._logger = logger;
            this._mapper = mapper;
            this._patrolData = patrolData;
            this._patrolImage = patrolImage;
            this._productData = productData;
            this._waterAnalysis = waterAnalysis;
            this._devicePatrol = devicePatrol;
            this._techniquePatrol = techniquePatrol;
            this._opsItem = opsItem;
            this._typeOps = typeOps;
        }

        public async Task<bool> IsExist(Expression<Func<PatrolDataModel, bool>> predicate)
        {
            var data = await _patrolData.Find(predicate).CountAsync();
            if (data <= 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        /// <summary>
        /// 创建巡检单
        /// </summary>
        /// <param name="account">操作人</param>
        /// <param name="Id">操作人标识，用来防止同一个用户在短时间内频繁创建巡检单</param>
        /// <param name="req">巡检单内容</param>
        /// <returns></returns>
        public async Task<BaseResponse> AddPatrolDataAsync(string account, int Id, PatrolDataAddDto req)
        {
            try
            {
                var patrolId = DateTime.Now.ToString("yyyyMMddHHmm");
                patrolId += Id.ToString();
                var data = await _patrolData.Find(a => a.Id == patrolId).FirstOrDefaultAsync();
                if (data != null)
                {
                    return new BaseResponse { Success = false, Message = "用户不能在短时间内创建多个巡检单" };
                }
                var entity = _mapper.Map<PatrolDataModel>(req);
                entity.Id = patrolId;
                entity.Create = account;
                entity.Dt = DateTime.Now;
                await _patrolData.AddAsync(entity);
                _logger.LogInformation($"{account}创建标识为{entity.Id}的巡检单成功");
                return new HandleResponse<string>() { Key = entity.Id, Success = true, Message = "创建巡检单成功" };
            }
            catch (Exception ex)
            {
                _logger.LogError($"{account}创建巡检单失败，失败原因{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "用户创建巡检单失败" };
            }
        }
        /// <summary>
        /// 添加巡检图片
        /// </summary>
        /// <param name="account">操作人</param>
        /// <param name="req">图片地址</param>
        /// <returns></returns>
        public async Task<BaseResponse> AddPatrolImageAsync(string account, PatrolImageAddDto req)
        {
            try
            {
                var entity = _mapper.Map<PatrolImageModel>(req);
                //entity.PatrolId = req.PatrolId;
                await _patrolImage.AddAsync(entity);
                _logger.LogInformation($"{account}添加标识为{req.PatrolId}的巡检图片成功");
                return new BaseResponse { Success = true, Message = "上传巡检图片成功" };
            }
            catch (Exception ex)
            {
                _logger.LogError($"{account}上传巡检图片失败，失败原因{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "用户上传巡检图片失败" };
            }
        }
        /// <summary>
        /// 添加巡检数据
        /// </summary>
        /// <param name="account">操作人</param>
        /// <param name="req">巡检数据</param>
        /// <returns></returns>
        public async Task<BaseResponse> AddPatrolItemAsync(string account, PatrolItemAddDto req)
        {
            string strType = "";
            try
            {
                switch (req.Type)
                {
                    case 0:
                        var entity = new ProductDataModel() { PatrolId = req.PatrolId, Content = req.Content };
                        await _productData.AddAsync(entity);
                        strType = "生产数据";
                        break;
                    case 1:
                        var water = new WaterAnalysisModel { PatrolId = req.PatrolId, Content = req.Content };
                        await _waterAnalysis.AddAsync(water);
                        strType = "水质分析数据";
                        break;
                    case 2:
                        strType = "设备巡检";
                        var device = new DevicePatrolModel { PatrolId = req.PatrolId, Content = req.Content };
                        await _devicePatrol.AddAsync(device);
                        break;
                    case 3:
                        strType = "工艺巡检";
                        var technique = new TechniquePatrolModel { PatrolId = req.PatrolId, Content = req.Content };
                        await _techniquePatrol.AddAsync(technique);
                        break;
                    default:
                        return new BaseResponse { Success = false, Message = "巡检类型不存在" };
                }
                _logger.LogInformation($"{account}添加巡检编号为{req.PatrolId}的{strType}成功");
                return new BaseResponse { Success = true, Message = $"添加{strType}成功" };
            }
            catch (Exception ex)
            {
                _logger.LogError($"{account}添加巡检编号为{req.PatrolId}的{strType}失败，失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = $"添加{strType}失败，请联系管理员" };
            }
        }

        /// <summary>
        /// 删除巡检数据
        /// </summary>
        /// <param name="account">操作人</param>
        /// <param name="Id">巡检单号</param>
        ///<param name="path">图片保存的目录路径</param>
        /// <returns>删除成功后需要处理巡检图片</returns>
        public async Task<BaseResponse> DeletePatrolDataAsync(string account, string Id, string path)
        {
            try
            {
                var data = await _patrolData.FindAsync(Id);
                if (data == null)
                {
                    return new BaseResponse { Success = false, Message = "输入的巡检单不存在" };
                }
                var image = await _patrolImage.FindAsync(Id);
                string[] url = null;
                if (image != null)
                {
                    //url = JsonConvert.DeserializeObject<List<string>>(image.Url);
                    url = image.Url.Split(';');
                }
                await _patrolData.RemoveAsync(data);
                //删除成功清除巡检图片
                if (url != null)
                {
                    foreach (var item in url)
                    {
                        var imagePath = Path.Combine(path, item);
                        if (File.Exists(imagePath))
                        {
                            File.Delete(imagePath);
                        }
                    }
                }
                _logger.LogInformation($"{account}删除巡检单号为{Id}的巡检单成功");
                return new BaseResponse { Success = true, Message = "删除巡检单成功" };
            }
            catch (Exception ex)
            {
                _logger.LogError($"{account}删除巡检单号为{Id}的巡检单失败，失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "删除巡检单失败，请联系管理员" };
            }
        }
        /// <summary>
        ///获取巡检项目,需要和类型巡检项目合并 
        /// </summary>
        /// <param name="req">巡检类型</param>
        /// <param name="typeId">设备类型标识</param>
        /// <returns></returns>
        public async Task<BaseResponse> GetPatrolItemAsync(PatrolItemRequest req, int typeId)
        {
            List<TypeOpsItemDto> patrolItems = new List<TypeOpsItemDto>();
            var opsItem = await _opsItem.Find(a => a.OpsType == (OpsType)req.OpsType).ToListAsync();
            var ops = _mapper.Map<List<TypeOpsItemDto>>(opsItem);
            var typeOpsItem = await _typeOps.Find(a => a.OpsType == (OpsType)req.OpsType && a.TypeId == typeId).ToListAsync();
            var typeOps = _mapper.Map<List<TypeOpsItemDto>>(typeOpsItem);
            patrolItems.AddRange(ops);
            patrolItems.AddRange(typeOps);
            return new BResponse<IEnumerable<TypeOpsItemDto>> { Success = true, Message = "获取数据成功", Data = patrolItems };
        }

        /// <summary>
        /// 根据巡检单号获取巡检数据
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<BaseResponse> GetPatrolDataByIdAsync(string Id)
        {
            var data =await _patrolData.FindWithPatrolData(a => a.Id==Id).FirstOrDefaultAsync();
            if (data==null)
            {
                return new BaseResponse { Success = false, Message = "输入的巡检单不存在" };
            }                
            var patrolData = _mapper.Map<PatrolDataDto>(data);
            var ret = new BResponse<PatrolDataDto>() { Success=true,Message="获取数据成功",Data=patrolData};
            return ret;
        }
        /// <summary>
        /// 获取巡检数据
        /// </summary>
        /// <param name="users">用户列表</param>
        /// <param name="req">查询条件</param>
        /// <returns>返回用户巡检数据</returns>
        public async Task<BaseResponse> GetPatrolDataPageAsync(List<string> users, PatrolDataRequest req)
        {
            var data = _patrolData.FindWithPatrolData(a => a.CreateTime >= req.BeginTime && a.CreateTime <= req.EndTime);
            data = data.Where(a => users.Contains(a.Create));
            if (!string.IsNullOrWhiteSpace(req.UserName))
            {
                data = data.Where(a => a.CreateName==req.UserName);
            }
            int count = data.Count();
            string OrderExpression = "";
            if (string.IsNullOrEmpty(req.OrderBy))
            {
                OrderExpression = "Id Asc";
            }
            else
            {
                OrderExpression = string.Format("{0} {1}", req.OrderBy, req.OrderType);
            }
            var list = await data.OrderBy(OrderExpression).Skip((req.PageNo - 1) * req.PageSize).Take(req.PageSize).ToListAsync();
            var patrolData = _mapper.Map<List<PatrolDataDto>>(list);
            var ret = new BasePageResponse<List<PatrolDataDto>>();
            ret.Success = true;
            ret.Message = "获取数据成功";
            ret.PageSize = req.PageSize;
            ret.CurrentPage = req.PageNo;
            ret.Count = count;
            ret.TotalPage = (int)Math.Ceiling((decimal)count / req.PageSize);
            ret.Data = patrolData;
            return ret;
        }
    }
}
