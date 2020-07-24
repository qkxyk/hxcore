using System;
using System.Collections.Generic;
using System.IO;
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
    public class TypeImageService : ITypeImageService
    {
        private readonly ILogger<TypeImageService> _log;
        private readonly IMapper _mapper;
        private readonly ITypeImageRepository _ti;
        private readonly ITypeRepository _tr;

        public TypeImageService(ILogger<TypeImageService> log, IMapper mapper, ITypeImageRepository ti, ITypeRepository tr)
        {
            this._log = log;
            this._mapper = mapper;
            this._ti = ti;
            this._tr = tr;
        }
        public async Task<bool> IsExist(Expression<Func<TypeImageModel, bool>> predicate)
        {
            var ret = await _ti.Find(predicate).FirstOrDefaultAsync();
            if (ret == null)
            {
                return false;
            }
            return true;
        }

        public async Task<BaseResponse> GetTypeImage(int typeId)
        {
            var ret = await _ti.Find(a => a.TypeId == typeId).ToListAsync();
            var dtos = _mapper.Map<List<TypeImageData>>(ret);
            var rm = new BResponse<List<TypeImageData>> { Success = true, Message = "获取数据成功", Data = dtos };
            return rm;
        }
        public async Task<BaseResponse> AddTypeImage(int typeId, TypeImageAddViewModel req, string account, string path)
        {
            //验证类型是否可以添加
            var t = await _tr.FindAsync(typeId);
            if (t.Status == TypeStatus.Root)
            {
                return new BaseResponse { Success = false, Message = "目录节点类型不能添加具体数据" };
            }
            //验证是否重名
            var ret = await _ti.Find(a => a.TypeId == typeId && a.ImageName == req.ImageName).FirstOrDefaultAsync();
            if (ret != null)
            {
                return new BaseResponse { Success = false, Message = "该类型下已存在相同名称的图片" };
            }
            try
            {
                var dto = _mapper.Map<TypeImageModel>(req);
                dto.TypeId = typeId;
                dto.Url = path;
                dto.Create = account;
                await _ti.AddAsync(dto);
                _log.LogInformation($"{account}添加类型图片{dto.Id},名称为{dto.ImageName}成功");
                return new HandleResponse<int> { Success = true, Message = "添加类型图片成功", Key = dto.Id };
            }
            catch (Exception ex)
            {
                _log.LogError($"{account}添加类型图片失败，失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "添加类型图片失败" };
            }
        }

        //Controller中验证是否存在
        public async Task<BaseResponse> UpdateTypeImage(TypeImageUpdateViewModel req, string account)
        {
            var dto = await _ti.FindAsync(req.Id);
            if (dto == null)
            {
                return new BaseResponse { Success = false, Message = "输入的标示不存在" };
            }
            //要更新的文件是否重名
            var ret = await _ti.Find(a => a.TypeId == req.TypeId && a.ImageName == req.ImageName).FirstOrDefaultAsync();
            if (ret != null && ret.Id != req.Id)
            {
                return new BaseResponse { Success = false, Message = "该类型已存在相同名称的升级文件" };
            }
            try
            {
                _mapper.Map(req, dto);
                dto.Modify = account;
                dto.ModifyTime = DateTime.Now;
                await _ti.SaveAsync(dto);
                _log.LogInformation($"{account}修改Id为{req.Id}的类型图片成功");
                return new BaseResponse { Success = true, Message = "修改类型图片成功" };
            }
            catch (Exception ex)
            {
                _log.LogError($"{account}修改Id为{req.Id}的类型图片失败,失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "修改类型图片失败" };
            }
        }

        public async Task<BaseResponse> DeleteTypeImage(int Id, string account, string path)
        {
            var ret = await _ti.FindAsync(Id);
            if (ret == null)
            {
                return new BaseResponse { Success = false, Message = "输入的类型图片不存在" };
            }
            try
            {
                //先删除文件
                string url = Path.Combine(path, ret.Url);
                if (System.IO.File.Exists(url))
                {
                    System.IO.File.Delete(url);
                }
                await _ti.RemoveAsync(ret);
                _log.LogInformation($"{account}删除Id为｛Id｝图片成功");
                return new BaseResponse { Success = true, Message = "删除图片成功" };
            }
            catch (Exception ex)
            {
                _log.LogError($"{account}删除Id为{Id}的图片失败，失败原因：{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "删除图片失败" };
            }
        }
        public async Task<string> GetTypeGroupIdAsync(int id)
        {
            var ret = await _ti.GetTypeImageWithTypeAsync(a => a.Id == id);
            if (ret == null || ret.Type == null)
            {
                return null;
            }
            return ret.Type.GroupId;
        }
    }
}
