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
    public class TypeUpdateFileService : ITypeUpdateFileService
    {
        private readonly ILogger<TypeUpdateFileService> _log;
        private readonly IMapper _mapper;
        private readonly ITypeUpdateFileRepository _tu;
        private readonly ITypeRepository _tr;

        public TypeUpdateFileService(ILogger<TypeUpdateFileService> log, IMapper mapper, ITypeUpdateFileRepository tu, ITypeRepository tr)
        {
            this._log = log;
            this._mapper = mapper;
            this._tu = tu;
            this._tr = tr;
        }
        public async Task<bool> IsExist(Expression<Func<TypeUpdateFileModel, bool>> predicate)
        {
            var ret = await _tu.Find(predicate).FirstOrDefaultAsync();
            if (ret == null)
            {
                return false;
            }
            return true;
        }

        public async Task<BaseResponse> AddTypeUpdateFile(int typeId, TypeUpdateFileAddViewModel req, string account, string url)
        {
            //验证类型是否可以添加
            var t = await _tr.FindAsync(typeId);
            if (t.Status == TypeStatus.Root)
            {
                return new BaseResponse { Success = false, Message = "目录节点类型不能添加具体数据" };
            }
            //是否存在重名
            var ret = await _tu.Find(a => a.TypeId == typeId && a.Name == req.Name).FirstOrDefaultAsync();
            if (ret != null)
            {
                return new BaseResponse { Success = false, Message = "已存在相同名称的升级文件" };
            }
            try
            {
                var dto = _mapper.Map<TypeUpdateFileModel>(req);
                dto.Create = account;
                dto.Url = url;
                dto.TypeId = typeId;
                await _tu.AddAsync(dto);
                _log.LogInformation($"{account}添加Id为{dto.Id}名称为{dto.Name}的升级文件成功");
                return new HandleResponse<int> { Success = true, Message = "添加升级文件成功", Key = dto.Id };
            }
            catch (Exception ex)
            {
                _log.LogError($"{account}添加升级文件{req.Name}失败，失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "添加失败文件失败，请联系管理员" };
            }
        }

        public async Task<BaseResponse> DeleteUpdateFile(int Id, string account, string path)
        {
            var ret = await _tu.FindAsync(Id);
            if (ret == null)
            {
                return new BaseResponse { Success = false, Message = "输入的文件编号不存在" };
            }
            try
            {
                //先删除文件
                string url = Path.Combine(path, ret.Url);
                if (System.IO.File.Exists(url))
                {
                    System.IO.File.Delete(url);
                }
                await _tu.RemoveAsync(ret);
                _log.LogInformation($"{account}删除文件{Id}成功");
                return new BaseResponse { Success = true, Message = "删除文件成功" };
            }
            catch (Exception ex)
            {
                _log.LogError($"{account}删除文件{Id}失败，失败原因{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "删除文件失败，请联系管理员" };
            }
        }
        public async Task<BaseResponse> UpdateTypeUpdateFile(TypeUpdateFileUpdateViewModel req, string account)
        {
            var dto = await _tu.FindAsync(req.Id);
            if (dto == null)
            {
                return new BaseResponse { Success = false, Message = "输入的标示不存在" };
            }
            //要更新的文件是否重名
            var ret = await _tu.Find(a => a.TypeId == req.TypeId && a.Name == req.Name).FirstOrDefaultAsync();
            if (ret != null && ret.Id != req.Id)
            {
                return new BaseResponse { Success = false, Message = "该类型已存在相同名称的升级文件" };
            }
            try
            {
                _mapper.Map(req, dto);
                dto.Modify = account;
                dto.ModifyTime = DateTime.Now;
                await _tu.SaveAsync(dto);
                _log.LogInformation($"{account}修改Id为{req.Id}的升级文件成功");
                return new BaseResponse { Success = true, Message = "修改升级文件成功" };
            }
            catch (Exception ex)
            {
                _log.LogError($"{account}修改Id为{req.Id}的升级文件失败,失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "修改升级文件失败，请联系管理员" };
            }
        }

        public async Task<BaseResponse> GetFile(int Id)
        {
            var ret = await _tu.FindAsync(Id);
            if (ret == null)
            {
                return new BaseResponse { Success = false, Message = "输入的升级文件标示不存在" };
            }
            var dto = _mapper.Map<TypeUpdateFileData>(ret);
            var rm = new BResponse<TypeUpdateFileData> { Success = true, Message = "获取数据成功", Data = dto };
            return rm;
        }

        public async Task<BaseResponse> GetTypeUpdateFile(int TypeId)
        {
            var ret = await _tu.Find(a => a.TypeId == TypeId).ToListAsync();
            var dtos = _mapper.Map<List<TypeUpdateFileData>>(ret);
            var rm = new BResponse<List<TypeUpdateFileData>> { Success = true, Message = "获取数据成功", Data = dtos };
            return rm;
        }
        public async Task<string> GetTypeGroupIdAsync(int FileId)
        {
            var ret = await _tu.GetTypeUpdateFileWithTypeAsync(a => a.Id == FileId);
            if (ret == null || ret.Type == null)
            {
                return null;
            }
            else
            {
                return ret.Type.GroupId;
            }
        }
    }
}
