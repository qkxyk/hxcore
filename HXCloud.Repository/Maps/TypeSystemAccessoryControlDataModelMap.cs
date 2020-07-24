using System;
using System.Collections.Generic;
using System.Text;
using HXCloud.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HXCloud.Repository.Maps
{
    public class TypeSystemAccessoryControlDataModelMap : BaseModelMap<TypeSystemAccessoryControlDataModel>
    {
        public override void Configure(EntityTypeBuilder<TypeSystemAccessoryControlDataModel> builder)
        {
            builder.ToTable("TypeSystemAccessoryControlData").HasKey(a => a.Id);
            builder.HasOne(a => a.TypeDataDefine).WithMany(a => a.TypeSystemAccessoryControlDatas).HasForeignKey(a => a.DataDefineId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(a => a.SystemAccessory).WithMany(a => a.TypeSystemAccessoryControlDatas).HasForeignKey(a => a.AccessoryId).OnDelete(DeleteBehavior.Cascade);
            base.Configure(builder);
        }
    }
}
