using System;
using System.Collections.Generic;
using System.Text;
using HXCloud.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HXCloud.Repository.Maps
{
    public class TypeAccessoryControlDataModelMap : BaseModelMap<TypeAccessoryControlDataModel>
    {
        public override void Configure(EntityTypeBuilder<TypeAccessoryControlDataModel> builder)
        {
            builder.ToTable("TypeAccessoryControlData").HasKey(a => a.Id);
            builder.HasOne(a => a.TypeDataDefine).WithMany(a => a.TypeAccessoryControlDatas).HasForeignKey(a => a.DataDefineId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(a => a.TypeAccessory).WithMany(a => a.TypeAccessoryControlDatas).HasForeignKey(a => a.AccessoryId).OnDelete(DeleteBehavior.Cascade);
            base.Configure(builder);
        }
    }
}
