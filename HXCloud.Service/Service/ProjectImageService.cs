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
    public class ProjectImageService : IProjectImageService
    {
        private readonly ILogger<ProjectImageService> _log;
        private readonly IMapper _mapper;
        private readonly IProjectImageRepository _pi;

        public ProjectImageService(ILogger<ProjectImageService> log, IMapper mapper, IProjectImageRepository pi)
        {
            this._log = log;
            this._mapper = mapper;
            this._pi = pi;
        }
        public async Task<bool> IsExist(Expression<Func<ProjectImageModel, bool>> predicate)
        {
            var data = await _pi.Find(predicate).FirstOrDefaultAsync();
            if (data == null)
            {
                return false;
            }
            return true;
        }

        public async Task<string> GetProjectGroupIdAsync(int Id)
        {
            var data = await _pi.GetWithProject(Id);
            if (data == null || data.project == null)
            {
                return null;
            }
            return data.project.GroupId;
        }

        public async Task<BaseResponse> AddProjectImageAsync(ProjectImageAddDto req, string url, string account)
        {
            try
            {
                var entity = _mapper.Map<ProjectImageModel>(req);
                entity.Create = account;
                entity.url = url;
                await _pi.AddAsync(entity);
                _log.LogInformation($"{account}添加图片成功，图片标示为{entity.Id}");
                return new BResponse<int> { Success = true, Message = "添加图片成功", Data = entity.Id };
            }
            catch (Exception ex)
            {
                _log.LogError($"{account}添加图片失败，失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "添加图片失败" };
            }
        }

        public async Task<BaseResponse> RemoveProjectImageAsync(int Id, string account, string path)
        {
            var ret = await _pi.FindAsync(Id);
            if (ret == null)
            {
                return new BaseResponse { Success = false, Message = "输入的图片不存在" };
            }
            try
            {
                //先删除文件
                string url = Path.Combine(path, ret.url);
                if (System.IO.File.Exists(url))
                {
                    System.IO.File.Delete(url);
                }
                await _pi.RemoveAsync(ret);
                _log.LogInformation($"{account}删除Id为｛Id｝图片成功");
                return new BaseResponse { Success = true, Message = "删除图片成功" };
            }
            catch (Exception ex)
            {
                _log.LogError($"{account}删除Id为{Id}的图片失败，失败原因：{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "删除图片失败" };
            }
        }

        public async Task<BaseResponse> GetImageAsync(int Id)
        {
            var data = await _pi.FindAsync(Id);
            if (data == null)
            {
                return new BaseResponse { Success = false, Message = "图片不存在" };
            }
            var dto = _mapper.Map<ProjectImageData>(data);
            return new BResponse<ProjectImageData> { Success = true, Message = "获取数据成功", Data = dto };
        }
        public async Task<BaseResponse> GetProjectImageAsync(int pId)
        {
            var data = await _pi.Find(a => a.ProjectId == pId).ToListAsync();
            var dtos = _mapper.Map<List<ProjectImageData>>(data);
            return new BResponse<List<ProjectImageData>> { Success = true, Message = "获取数据成功", Data = dtos };
        }
    }
}
