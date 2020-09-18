using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using HXCloud.Service;
using HXCloud.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using OfficeOpenXml;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HXCloud.APIV2.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class DataDefineLibraryController : Controller
    {
        private readonly ILogger<DataDefineLibraryController> _log;
        private readonly IConfiguration _config;
        private readonly IDataDefineLibraryService _dls;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public DataDefineLibraryController(ILogger<DataDefineLibraryController> log, IConfiguration config, IDataDefineLibraryService dls, IWebHostEnvironment webHostEnvironment)
        {
            this._log = log;
            this._config = config;
            this._dls = dls;
            this._webHostEnvironment = webHostEnvironment;
        }

        [HttpPost]
        public async Task<ActionResult<BaseResponse>> Add(DataDefineLibraryAddDto req)
        {
            //超级管理员有权限
            var GroupId = User.Claims.FirstOrDefault(a => a.Type == "GroupId").Value;
            var isAdmin = User.Claims.FirstOrDefault(a => a.Type == "IsAdmin").Value.ToLower() == "true" ? true : false;
            string Code = User.Claims.FirstOrDefault(a => a.Type == "Code").Value;
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;

            if (!(isAdmin && Code == _config["Group"]))
            {
                return Unauthorized("用户没有权限添加数据定义库");
            }
            var rm = await _dls.AddDataDefineAsync(req, Account);
            return rm;
        }
        [HttpPut]
        public async Task<ActionResult<BaseResponse>> Update([FromBody]DataDefineLibraryUpdateDto req)
        {
            //超级管理员有权限
            var GroupId = User.Claims.FirstOrDefault(a => a.Type == "GroupId").Value;
            var isAdmin = User.Claims.FirstOrDefault(a => a.Type == "IsAdmin").Value.ToLower() == "true" ? true : false;
            string Code = User.Claims.FirstOrDefault(a => a.Type == "Code").Value;
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;

            if (!(isAdmin && Code == _config["Group"]))
            {
                return Unauthorized("用户没有权限修改数据定义库");
            }
            var rm = await _dls.UpdateDataDefineAsync(req, Account);
            return rm;
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<BaseResponse>> Get(int Id)
        {
            var isAdmin = User.Claims.FirstOrDefault(a => a.Type == "IsAdmin").Value.ToLower() == "true" ? true : false;
            if (!isAdmin)//管理员可以获取数据定义库数据
            {
                return Unauthorized("用户没有权限获取数据定义库");
            }
            var rm = await _dls.GetDataDefineLibraryAsync(Id);
            return rm;
        }

        [HttpGet]
        public async Task<ActionResult<BaseResponse>> Get([FromQuery] DataDefineLibraryPageRequest req)
        {
            var isAdmin = User.Claims.FirstOrDefault(a => a.Type == "IsAdmin").Value.ToLower() == "true" ? true : false;
            if (!isAdmin)//管理员可以获取数据定义库数据
            {
                return Unauthorized("用户没有权限获取数据定义库");
            }
            var rm = await _dls.GetDataDefineLibrarysAsync(req);
            return rm;
        }
        [HttpDelete("{Id}")]
        public async Task<ActionResult<BaseResponse>> Delete(int Id)
        {
            //超级管理员有权限
            var GroupId = User.Claims.FirstOrDefault(a => a.Type == "GroupId").Value;
            var isAdmin = User.Claims.FirstOrDefault(a => a.Type == "IsAdmin").Value.ToLower() == "true" ? true : false;
            string Code = User.Claims.FirstOrDefault(a => a.Type == "Code").Value;
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            if (!(isAdmin && Code == _config["Group"]))
            {
                return Unauthorized("用户没有权限删除数据定义库");
            }
            var rm = await _dls.DeleteDataDefineAsync(Id, Account);
            return rm;
        }

        //批量导入功能
        //此功能引用库为收费库
        [HttpPost("Import")]
        public async Task<ActionResult> Import(IFormFile excelfile)
        {
            //超级管理员有权限
            var GroupId = User.Claims.FirstOrDefault(a => a.Type == "GroupId").Value;
            var isAdmin = User.Claims.FirstOrDefault(a => a.Type == "IsAdmin").Value.ToLower() == "true" ? true : false;
            string Code = User.Claims.FirstOrDefault(a => a.Type == "Code").Value;
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            if (!(isAdmin && Code == _config["Group"]))
            {
                return Unauthorized("用户没有权限删除数据定义库");
            }
            string sWebRootFolder = _webHostEnvironment.WebRootPath;
            string sFileName = $"{Guid.NewGuid()}.xlsx";
            string path = Path.Combine(sWebRootFolder, "Excel");
            //如果路径不存在，创建路径
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            FileInfo file = new FileInfo(Path.Combine(path, sFileName));
            try
            {
                using (FileStream fs = new FileStream(file.ToString(), FileMode.Create))
                {
                    //excelfile.CopyTo(fs);
                    //fs.Flush();
                    await excelfile.CopyToAsync(fs);
                }
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (ExcelPackage package = new ExcelPackage(file))
                {
                    //StringBuilder sb = new StringBuilder();
                    List<DataDefineLibraryAddDto> list = new List<DataDefineLibraryAddDto>();
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    int rowCount = worksheet.Dimension.Rows;
                    int ColCount = worksheet.Dimension.Columns;
                    //bool bHeaderRow = true;
                    for (int row = 2; row <= rowCount; row++)
                    {
                        DataDefineLibraryAddDto d = new DataDefineLibraryAddDto();
                        //for (int col = 1; col <= ColCount; col++)
                        //{

                        d.DataKey = worksheet.Cells[row, 2].Value == null ? "" : worksheet.Cells[row, 2].Value.ToString();
                        d.Unit = worksheet.Cells[row, 8].Value == null ? "string" : worksheet.Cells[row, 8].Value.ToString();
                        //}
                        list.Add(d);
                    }
                    return null;
                    //return Content(sb.ToString());
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        [HttpPost("Excel")]
        //批量导入建议采用此方案
        public async Task<ActionResult> Excel(IFormFile excel)
        {
            //超级管理员有权限
            var GroupId = User.Claims.FirstOrDefault(a => a.Type == "GroupId").Value;
            var isAdmin = User.Claims.FirstOrDefault(a => a.Type == "IsAdmin").Value.ToLower() == "true" ? true : false;
            string Code = User.Claims.FirstOrDefault(a => a.Type == "Code").Value;
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            if (!(isAdmin && Code == _config["Group"]))
            {
                return Unauthorized("用户没有权限删除数据定义库");
            }
            string[] extensionName = new string[] { ".XLSX", ".XLS" };
            string sWebRootFolder = _webHostEnvironment.WebRootPath;
            string sFileName = $"{Guid.NewGuid()}.xlsx";
            string path = Path.Combine(sWebRootFolder, "Excel");
            string p = Path.Combine(path, sFileName);
            //如果路径不存在，创建路径
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            FileInfo filePath = new FileInfo(Path.Combine(path, sFileName));
            try
            {
                using (FileStream fs = new FileStream(filePath.ToString(), FileMode.Create))
                {
                    //excelfile.CopyTo(fs);
                    //fs.Flush();
                    await excel.CopyToAsync(fs);
                }
                using (FileStream file = new FileStream(p, FileMode.Open, FileAccess.Read))
                {
                    //获取需要被导入的excel工作薄
                    IWorkbook workBook = new XSSFWorkbook(file);
                    //if (workBook.NumberOfSheets > 0)
                    //{

                    //}

                    for (int v = 0; v < workBook.NumberOfSheets; v++)
                    {
                        var sheet = workBook.GetSheetAt(v);
                        var head = sheet.Header.ToString();
                        List<DataDefineLibraryAddDto> list = new List<DataDefineLibraryAddDto>();
                        for (int i = 0; i < sheet.LastRowNum + 1; i++)
                        {
                            var row = sheet.GetRow(i);
                            DataDefineLibraryAddDto d = new DataDefineLibraryAddDto();
                            if (row != null)
                            {
                                ////LastCellNum 是当前行的总列数
                                //for (int j = 0; j < row.LastCellNum; j++)
                                //{
                                //    //读取该行的第j列数据
                                //    string value = row.GetCell(j).ToString();
                                //    Console.Write(value.ToString() + " ");
                                //}
                                //Console.WriteLine("\n");
                                d.DataKey = row.GetCell(2).StringCellValue;
                                d.Unit = row.GetCell(8).StringCellValue;
                                list.Add(d);
                            }

                        }
                    }
                }
            }
            catch
            {

            }
            return null;
        }
    }
}
