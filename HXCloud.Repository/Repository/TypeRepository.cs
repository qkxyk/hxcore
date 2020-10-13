﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using HXCloud.Model;
using Microsoft.EntityFrameworkCore;

namespace HXCloud.Repository
{
    public class TypeRepository : BaseRepository<TypeModel>, ITypeRepository
    {
        /// <summary>
        /// 获取类型和类型的子节点
        /// </summary>
        /// <param name="predicate">类型表达式</param>
        /// <returns>返回满足条件的第一个类型</returns>
        public async Task<TypeModel> FindAsync(Expression<Func<TypeModel, bool>> predicate)
        {
            var data = await _db.Types.Include(a => a.Child).Where(predicate).FirstOrDefaultAsync();
            return data;
        }
        public async Task CopyTypeAsync(int sourceId, TypeModel target)
        {
            //获取配件和控制项
            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    await _db.Types.AddAsync(target);
                    //await _db.TypeModules.AddAsync(target);
                    #region 处理类型数据定义
                    //定义数据定义
                    List<TypeDataDefineModel> dataDefineList = new List<TypeDataDefineModel>();
                    var dataDefines = await _db.TypeDataDefines.Where(a => a.TypeId == sourceId).ToListAsync();
                    foreach (var item in dataDefines)
                    {
                        var data = new TypeDataDefineModel
                        {
                            Type = target,
                            Create = target.Create,
                            CreateTime = target.CreateTime,
                            AutoControl = item.AutoControl,
                            DataKey = item.DataKey,
                            DataName = item.DataName,
                            Unit = item.Unit,
                            Category = item.Category,
                            DataType = item.DataType,
                            OutKey = item.OutKey,
                            DefaultValue = item.DefaultValue,
                            Format = item.Format,
                            Model = item.Model
                        };
                        dataDefineList.Add(data);
                    }
                    await _db.TypeDataDefines.AddRangeAsync(dataDefineList);

                    #endregion
                  
                    #region 处理类型模式
                    List<TypeSchemaModel> typeSchema = new List<TypeSchemaModel>();
                    var schema = await _db.TypeSchemas.Include(a => a.Child).Where(a => a.TypeId == sourceId && a.Parent == null).ToListAsync();
                    foreach (var item in schema)
                    {
                        TypeSchemaModel data = new TypeSchemaModel()
                        {
                            Create = target.Create,
                            CreateTime = target.CreateTime,
                            Key = item.Key,
                            Name = item.Name,
                            SchemaType = item.SchemaType,
                            Value = item.Value,
                            Type = target
                        };
                        if (item.Child != null && item.Child.Count > 0)
                        {
                            HandleSchema(item, target.Create, target, data);
                        }
                        typeSchema.Add(data);
                    }
                    await _db.TypeSchemas.AddRangeAsync(typeSchema);
                    #endregion
                  
                    #region 处理类型配置
                    var typeConfig = await _db.TypeConfigs.Where(a => a.TypeId == sourceId).ToListAsync();
                    List<TypeConfigModel> typeConfigList = new List<TypeConfigModel>();
                    foreach (var item in typeConfig)
                    {
                        TypeConfigModel data = new TypeConfigModel
                        {
                            Create = target.Create,
                            CreateTime = target.CreateTime,
                            DataName = item.DataName,
                            DataType = item.DataType,
                            DataValue = item.DataValue,
                            Type = target
                        };
                        typeConfigList.Add(data);
                    }
                    await _db.TypeConfigs.AddRangeAsync(typeConfigList);
                    #endregion
                  
                    #region 处理类型参数
                    var typeArgument = await _db.TypeArguments.Where(a => a.TypeId == sourceId).ToListAsync();
                    List<TypeArgumentModel> typeArgumentList = new List<TypeArgumentModel>();
                    foreach (var item in typeArgument)
                    {
                        var key = dataDefines.Where(a => a.Id == item.DefineId).Select(a => a.DataKey).FirstOrDefault();
                        if (key == null || "" == key)  //如果key不存在则跳过
                        {
                            continue;
                        }
                        var td = dataDefineList.Where(a => a.DataKey == key).FirstOrDefault();
                        TypeArgumentModel data = new TypeArgumentModel
                        {
                            Create = target.Create,
                            CreateTime = target.CreateTime,
                            Type = target,
                            Category = item.Category,
                            Name = item.Name,
                            TypeDataDefine = td
                        };
                        typeArgumentList.Add(data);
                    }
                    await _db.TypeArguments.AddRangeAsync(typeArgumentList);
                    #endregion

                    #region 处理类型统计
                    var typeStat = await _db.TypeStatisticsInfos.Where(a => a.TypeId == sourceId).ToListAsync();
                    List<TypeStatisticsInfoModel> typeStatisticsList = new List<TypeStatisticsInfoModel>();
                    foreach (var item in typeStat)
                    {
                        TypeStatisticsInfoModel data = new TypeStatisticsInfoModel
                        {
                            Create = target.Create,
                            CreateTime = target.CreateTime,
                            Description = item.Description,
                            Type = target,
                            DataKey = item.DataKey,
                            DisplayType = item.DisplayType,
                            Filter = item.Filter,
                            FilterType = item.FilterType,
                            Name = item.Name,
                            ShowState = item.ShowState,
                            Standard = item.Standard,
                            StaticsType = item.StaticsType,
                            SUnit = item.SUnit
                        };
                        typeStatisticsList.Add(data);
                    }
                    await _db.TypeStatisticsInfos.AddRangeAsync(typeStatisticsList);
                    #endregion
                
                    #region 类型展示图标
                    List<TypeDisplayIconModel> typeDisplayIconList = new List<TypeDisplayIconModel>();
                    var icons = await _db.TypeDisplayIcons.Where(a => a.TypeId == sourceId).ToListAsync();
                    foreach (var item in icons)
                    {
                        var key = dataDefines.FirstOrDefault(a => a.Id == item.DataDefineId).DataKey;
                        var td = dataDefineList.Where(a => a.DataKey == key).FirstOrDefault();
                        TypeDisplayIconModel data = new TypeDisplayIconModel
                        {
                            Type = target,
                            Create = target.Create,
                            CreateTime = target.CreateTime,
                            Icon = item.Icon,
                            Name = item.Name,
                            Sn = item.Sn,
                            TypeDataDefine = td
                        };
                        typeDisplayIconList.Add(data);
                    }
                    await _db.TypeDisplayIcons.AddRangeAsync(typeDisplayIconList);
                    #endregion
                  
                    #region 类型总揽
                    List<TypeOverviewModel> typeOverviewList = new List<TypeOverviewModel>();
                    var typeOverview = await _db.TypeOverviews.Where(a => a.TypeId == sourceId).ToListAsync();
                    foreach (var item in typeOverview)
                    {
                        var key = dataDefines.FirstOrDefault(a => a.Id == item.TypeDataDefineId).DataKey;
                        var td = dataDefineList.Where(a => a.DataKey == key).FirstOrDefault();
                        TypeOverviewModel data = new TypeOverviewModel
                        {
                            Type = target,
                            Create = target.Create,
                            CreateTime = target.CreateTime,
                            Name = item.Name,
                            Sn = item.Sn,
                            TypeDataDefine = td
                        };
                        typeOverviewList.Add(data);
                    }
                    await _db.TypeOverviews.AddRangeAsync(typeOverviewList);
                    #endregion
                   
                    #region 处理类型模块
                    List<TypeModuleModel> typeModuleList = new List<TypeModuleModel>();
                    var typeModules = await _db.TypeModules.Include(a => a.ModuleControls).ThenInclude(a => a.TypeModuleFeedbacks).Where(a => a.TypeId == sourceId).ToListAsync();
                    foreach (var item in typeModules)
                    {
                        TypeModuleModel data = new TypeModuleModel
                        {
                            Type = target,
                            Create = target.Create,
                            CreateTime = target.CreateTime,
                            ModuleName = item.ModuleName,
                            ModuleType = item.ModuleType,
                            Sn = item.Sn
                        };
                        #region 处理类型模块控制数据
                        if (item.ModuleControls != null && item.ModuleControls.Count > 0)
                        {
                            data.ModuleControls = new List<TypeModuleControlModel>();
                            foreach (var ite in item.ModuleControls)
                            {
                                var key = dataDefines.FirstOrDefault(a => a.Id == ite.DataDefineId).DataKey;
                                var tdc = dataDefineList.Where(a => a.DataKey == key).FirstOrDefault();
                                TypeModuleControlModel dc = new TypeModuleControlModel()
                                {
                                    Create = target.Create,
                                    CreateTime = target.CreateTime,
                                    ControlName = ite.ControlName,
                                    DataValue = ite.DataValue,
                                    Formula = ite.Formula,
                                    Sn = ite.Sn,
                                    TypeDataDefine = tdc
                                };
                                #region 处理类型模块控制反馈数据
                                if (ite.TypeModuleFeedbacks != null && ite.TypeModuleFeedbacks.Count > 0)
                                {
                                    dc.TypeModuleFeedbacks = new List<TypeModuleFeedbackModel>();
                                    foreach (var it in ite.TypeModuleFeedbacks)
                                    {
                                        var fkey = dataDefines.FirstOrDefault(a => a.Id == ite.DataDefineId).DataKey;
                                        var tdf = dataDefineList.Where(a => a.DataKey == fkey).FirstOrDefault();
                                        var tfd = new TypeModuleFeedbackModel
                                        {
                                            Create = target.Create,
                                            CreateTime = target.CreateTime,
                                            Sn = it.Sn,
                                            TypeModuleControl = dc,
                                            TypeDataDefine = tdf
                                        };
                                        dc.TypeModuleFeedbacks.Add(tfd);
                                    }
                                }
                                #endregion
                                data.ModuleControls.Add(dc);
                            }
                        }
                        #endregion
                        typeModuleList.Add(data);
                    }
                    await _db.TypeModules.AddRangeAsync(typeModuleList);
                    #endregion

                    #region 处理类型更新文件
                    //获取类型更新文件

                    #endregion
                   
                    #region 处理类型工艺图片

                    #endregion
                    #region 处理类型配件
                    //target.TypeAccessories = new List<TypeAccessoryModel>();
                    //var dc = await _db.TypeAccessories.Include(a => a.TypeAccessoryControlDatas).Where(a => a.TypeId == sourceId).ToListAsync();
                    //foreach (var item in dc)
                    //{
                    //    TypeAccessoryModel tam = new TypeAccessoryModel
                    //    {
                    //        Type = target,
                    //        Create = target.Create,
                    //        ICON = item.ICON,
                    //        Name = item.Name
                    //    };
                    //    tam.TypeAccessoryControlDatas = new List<TypeAccessoryControlDataModel>();
                    //    foreach (var it in item.TypeAccessoryControlDatas)
                    //    {
                    //        TypeAccessoryControlDataModel tacm = new TypeAccessoryControlDataModel
                    //        {
                    //            TypeAccessory = tam,
                    //            AssociateDefineId = it.AssociateDefineId,
                    //            ControlName = it.ControlName,
                    //            Create = target.Create,
                    //            DataDefineId = it.DataDefineId,
                    //            DataValue = it.DataValue,
                    //            IState = it.IState,
                    //            SequenceIn = it.SequenceIn,
                    //            SequenceOut = it.SequenceOut
                    //        };
                    //        tam.TypeAccessoryControlDatas.Add(tacm);
                    //    }
                    //    target.TypeAccessories.Add(tam);
                    //}
                    #endregion

                    _db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
            }
        }
        private void HandleSchema(TypeSchemaModel schema, string Account, TypeModel t, TypeSchemaModel tar)
        {
            tar.Child = new List<TypeSchemaModel>();
            foreach (var item in schema.Child)
            {
                TypeSchemaModel data = new TypeSchemaModel()
                {
                    Create = t.Create,
                    CreateTime = t.CreateTime,
                    Key = item.Key,
                    Name = item.Name,
                    SchemaType = item.SchemaType,
                    Value = item.Value,
                    Type = t
                };
                tar.Child.Add(data);
                if (item.Child != null && item.Child.Count > 0)
                {
                    HandleSchema(item, Account, t, data);
                }
            }
        }
    }
}
