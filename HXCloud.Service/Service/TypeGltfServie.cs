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
    public class TypeGltfServie : ITypeGltfService
    {
        private readonly ILogger<TypeImageService> _log;
        private readonly IMapper _mapper;
        private readonly ITypeGltfRepository _ti;
        private readonly ITypeRepository _tr;

        public TypeGltfServie(ILogger<TypeImageService> log, IMapper mapper, ITypeGltfRepository ti, ITypeRepository tr)
        {
            this._log = log;
            this._mapper = mapper;
            this._ti = ti;
            this._tr = tr;
        }

        public Task<bool> IsExist(Expression<Func<TypeGltfModel, bool>> predicate)
        {
            throw new NotImplementedException();
        }
        public async Task<BaseResponse> GetTypeGltfAsync(int typeId)
        {
            var ret = await _ti.Find(a => a.TypeId == typeId).FirstOrDefaultAsync();
            var rm = new BResponse<string> { Success = true, Message = "获取数据成功", Data = ret==null?null:ret.Url };
            return rm;
        }
        public async Task<BaseResponse> AddTypeGltfAsync(int typeId, TypeGltfAddDto req, string account, string path)
        {
            //验证类型是否可以添加
            var t = await _tr.FindAsync(typeId);
            if (t.Status == TypeStatus.Root)
            {
                return new BaseResponse { Success = false, Message = "目录节点类型不能添加具体数据" };
            }
            ////验证是否重名
            //var ret = await _ti.Find(a => a.TypeId == typeId && a.GltfName == req.GltfName).FirstOrDefaultAsync();
            //if (ret != null)
            //{
            //    return new BaseResponse { Success = false, Message = "该类型下已存在相同名称的图片" };
            //}
            try
            {
                var dto = _mapper.Map<TypeGltfModel>(req);
                dto.TypeId = typeId;
                dto.Url = path;
                dto.Create = account;
                await _ti.AddAsync(dto);
                _log.LogInformation($"{account}添加类型3D文件{dto.Id}成功");
                return new HandleResponse<int> { Success = true, Message = "添加类型3D文件成功", Key = dto.Id };
            }
            catch (Exception ex)
            {
                _log.LogError($"{account}添加类型3D文件，失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "添加类型3D文件失败" };
            }
        }
        public async Task<BaseResponse> DeleteTypeGltfAsync(int typeId,string account, string path)
        {
            var ret = await _ti.FindAsync(a=>a.TypeId==typeId);
            if (ret == null)
            {
                return new BaseResponse { Success = false, Message = "输入的类型3D文件不存在" };
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
                _log.LogInformation($"{account}删除类型标识为｛Id｝3D文件成功");
                return new BaseResponse { Success = true, Message = "删除文件成功" };
            }
            catch (Exception ex)
            {
                _log.LogError($"{account}删除类型标识为｛Id｝3D文件失败，失败原因：{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "删除3D文件失败" };
            }
        }
        //public async Task<string> GetTypeGroupIdAsync(int id)
        //{
        //    var ret = await _ti.GetTypeImageWithTypeAsync(a => a.Id == id);
        //    if (ret == null || ret.Type == null)
        //    {
        //        return null;
        //    }
        //    return ret.Type.GroupId;
        //}
    }
}
