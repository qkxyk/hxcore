using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
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
        private readonly IMapper _mapper;
        private readonly IProjectRepository _pr;
        private readonly IDeviceRepository _dr;

        public RegionService(ILogger<RegionService> log, IRegionRepository rr, IMapper mapper, IProjectRepository pr, IDeviceRepository dr)
        {
            this._log = log;
            this._rr = rr;
            this._mapper = mapper;
            this._pr = pr;
            this._dr = dr;
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

        public async Task<BaseResponse> AddRegionAsync(string account, string groupId, RegionAddDto req)
        {
            //检测parentId是否合法，不合法置为null
            if (req.ParentId != null && "" == req.ParentId.Trim())
            {
                req.ParentId = null;
            }
            //统一组织的,统一层级区域名称不能重复
            var data = await _rr.Find(a => a.GroupId == groupId && a.ParentId == req.ParentId && a.Name == req.Name).FirstOrDefaultAsync();
            if (data != null)
            {
                return new BaseResponse { Success = false, Message = "该区域下已存在相同的区域名称" };
            }
            RegionModel parent = null;
            try
            {
                bool bP = false;
                string fullPath = null;
                string regionId = "101";
                //生成区域标示
                if (req.ParentId != null)     //有父区域标示
                {
                    //获取父节点
                    parent = await _rr.FindAsync(req.ParentId, groupId);
                    if (parent == null)
                    {
                        return new BaseResponse { Success = false, Message = "输入的父节点不存在" };
                    }
                    if (parent.DeleteId == null || "" == parent.DeleteId)//没有删除节点
                    {
                        //获取Id最大的节点
                        var bro = await _rr.Find(a => a.GroupId == groupId && a.ParentId == req.ParentId).OrderByDescending(a => a.Id).FirstOrDefaultAsync();
                        if (bro == null)
                        {
                            regionId = req.ParentId + "001";
                        }
                        else
                        {
                            string r = bro.Id;
                            int ir = Convert.ToInt32(r.Substring(r.Length - 3)) + 1;
                            regionId = r.Substring(0, r.Length - 3) + ir.ToString().PadLeft(3, '0');
                        }
                    }//父节点没有删除节点
                    else//父节点有删除节点
                    {
                        bP = true;
                        string[] strDelete = parent.DeleteId.Split(',');
                        regionId = strDelete[0];
                        string[] arr = new string[strDelete.Length - 1];
                        //删除数组第一个元素
                        Array.Copy(strDelete, 1, arr, 0, arr.Length);
                        parent.DeleteId = string.Join(",", arr);
                    }
                    fullPath = parent.FullPath + "/" + regionId;
                }//有父节点
                else//没有父节点
                {
                    req.ParentId = null;
                    if (req.RegionCode.Length != 3)
                    {
                        return new BaseResponse { Success = false, Message = "添加区域失败,区域的区域码必须为3位" };
                    }
                    regionId = req.RegionCode;
                    fullPath = regionId;
                    //检测是否添加过
                    var top = await _rr.FindAsync(regionId, groupId);
                    if (top != null)
                    {
                        return new BaseResponse { Success = false, Message = "此区域已存在，添加失败" };
                    }
                }
                var entity = _mapper.Map<RegionModel>(req);
                entity.GroupId = groupId;
                entity.Id = regionId;
                entity.Create = account;
                entity.FullPath = fullPath;
                await _rr.AddAsync(entity, parent, bP);
                _log.LogInformation($"{account}添加标示为{new { entity.Id, entity.GroupId }}的区域成功");
                return new HandleResponse<object> { Success = true, Message = "添加区域成功", Key = new { Id = regionId, GroupId = groupId } };
            }
            catch (Exception ex)
            {
                _log.LogError($"{account}添加区域失败，失败原因：{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "添加区域失败，请联系管理员" };
            }
        }

        public async Task<BaseResponse> UpdateRegionAsync(string account, string GroupId, RegionUpdateDto req)
        {
            var data = await _rr.FindAsync(req.Id, GroupId);
            if (data == null)
            {
                return new BaseResponse { Success = false, Message = "输入的区域标示不存在" };
            }
            //检测是否重名
            var d = await _rr.Find(a => a.ParentId == data.ParentId && a.GroupId == GroupId && a.Name == req.Name && a.Id != req.Id).FirstOrDefaultAsync();
            if (d != null)
            {
                return new BaseResponse { Success = false, Message = "修改区域信息失败，区域名称重复" };
            }
            try
            {
                var dto = _mapper.Map(req, data);
                dto.Modify = account;
                dto.ModifyTime = DateTime.Now;
                await _rr.SaveAsync(dto);
                _log.LogInformation($"{account}修改标示为{new { Id = req.Id, GroupId = GroupId }}的区域成功");
                return new BaseResponse { Success = true, Message = "修改区域信息成功" };
            }
            catch (Exception ex)
            {
                _log.LogError($"{account}修改标示为{new { Id = req.Id, GroupId = GroupId }}的区域失败，失败原因：{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "修改区域失败，请联系管理员" };
            }
        }
        public async Task<BaseResponse> DeleteRegionAsync(string account, string Id, string GroupId)
        {
            var data = await _rr.FindAsync(Id, GroupId);
            if (data == null)
            {
                return new BaseResponse { Success = false, Message = "输入的区域标示不存在" };
            }
            var d = await _rr.FindWithChildAsync(a => a.Id == Id && a.GroupId == GroupId);
            if (d.Child.Count > 0)
            {
                return new BaseResponse { Success = false, Message = "此区域下有子区域，不能删除" };
            }

            try
            {
                //检测区域是否使用，项目、设备和子区域
                var p = await _pr.Find(a => a.GroupId == GroupId && a.RegionId == Id).FirstOrDefaultAsync();
                if (p != null)
                {
                    return new BaseResponse { Success = false, Message = "此区域已被项目或者场站使用，不能删除" };
                }
                var device = await _dr.Find(a => a.GroupId == GroupId && a.RegionId == Id).FirstOrDefaultAsync();
                if (device != null)
                {
                    return new BaseResponse { Success = false, Message = "此区域已被设备使用，不能删除" };
                }
                RegionModel parent = null;
                if (data.Parent != null)//如果存在父节点，把当前的id加入到父节点已删除中
                {
                    parent = await _rr.FindAsync(data.ParentId, GroupId);
                    if (parent.DeleteId == null || parent.DeleteId.Trim() == "")
                    {
                        parent.DeleteId = Id;
                    }
                    else
                    {
                        parent.DeleteId += "," + Id;
                    }
                }
                await _rr.RemoveAsync(data, parent);
                _log.LogInformation($"{account}删除标示为{new { Id = Id, GroupId = GroupId }}的区域标示成功");
                return new BaseResponse { Success = true, Message = "删除区域成功" };
            }
            catch (Exception ex)
            {
                _log.LogError($"{account}删除标示为{new { Id = Id, GroupId = GroupId }}的区域失败，失败原因：{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "删除数据失败，请联系管理员" };
            }
        }
        //获取区域，递归出子区域
        public async Task<BaseResponse> GetRegionAsync(string Id, string GroupId)
        {
            var data = await _rr.FindWithChildAsync(a => a.Id == Id && a.GroupId == GroupId);
            if (data == null)
            {
                return new BaseResponse { Success = false, Message = "输入的区域标示不存在" };
            }
            var dto = _mapper.Map<RegionDto>(data);
            if (data.Child.Count > 0)
            {
                await GetChild(dto, Id, GroupId);
            }
            return new BResponse<RegionDto> { Success = true, Message = "获取区域数据成功", Data = dto };
        }

        public async Task GetChild(RegionDto td, string Id, string GroupId)
        {
            var ret = await _rr.Find(a => a.ParentId == Id && a.GroupId == GroupId).ToListAsync();
            foreach (var item in ret)
            {
                var dto = _mapper.Map<RegionDto>(item);
                td.Child.Add(dto);
                await GetChild(dto, item.Id, GroupId);
            }
        }

        public async Task<BaseResponse> GetGroupRegionAsync(string GroupId)
        {
            List<RegionDto> list = new List<RegionDto>();
            var data = await _rr.Find(a => a.Parent == null && a.GroupId == GroupId).ToListAsync();
            foreach (var item in data)
            {
                var dto = _mapper.Map<RegionDto>(item);
                await GetChild(dto, item.Id, GroupId);
                list.Add(dto);
            }
            return new BResponse<List<RegionDto>> { Success = true, Message = "获取数据成功", Data = list };
        }
    }
}
