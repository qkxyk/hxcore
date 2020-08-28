using System;
using System.Collections.Generic;
using System.Text;
using HXCloud.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HXCloud.Repository.Maps
{
    public class WarnModelMap : IEntityTypeConfiguration<WarnModel>
    {
        public void Configure(EntityTypeBuilder<WarnModel> builder)
        {
            builder.ToTable("Warn").HasKey(a => a.Id);
            builder.HasOne(a => a.Device).WithMany(a => a.Warns).HasForeignKey(a => a.DeviceSn).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(a => a.WarnCode).WithMany(a => a.Warn).HasForeignKey(a => a.Code).OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class WarnTypeModelMap : BaseModelMap<WarnTypeModel>
    {
        public override void Configure(EntityTypeBuilder<WarnTypeModel> builder)
        {
            builder.ToTable("WarnType").HasKey(a => a.Id);
            //builder.HasOne(a => a.Group).WithMany(a => a.WarnTypes).HasForeignKey(a => a.GroupId).OnDelete(DeleteBehavior.Cascade);
            base.Configure(builder);
        }
    }
    public class WarnCodeModelMap : BaseModelMap<WarnCodeModel>
    {
        public override void Configure(EntityTypeBuilder<WarnCodeModel> builder)
        {
            builder.ToTable("WarnCode").HasKey(a => a.Code);
            builder.HasOne(a => a.WarnType).WithMany(a => a.WarnCode).HasForeignKey(a => a.WarnTypeId).OnDelete(DeleteBehavior.Cascade);
            base.Configure(builder);
        }
    }
}
