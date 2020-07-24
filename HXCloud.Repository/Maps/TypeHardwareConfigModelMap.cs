using System;
using System.Collections.Generic;
using System.Text;
using HXCloud.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HXCloud.Repository.Maps
{
    public class TypeHardwareConfigModelMap : BaseModelMap<TypeHardwareConfigModel>
    {
        public override void Configure(EntityTypeBuilder<TypeHardwareConfigModel> builder)
        {
            builder.ToTable("TypeHardwareConfig").HasKey(a => a.Id);
            builder.HasOne(a => a.Type).WithMany(a => a.TypeHardwareConfig).HasForeignKey(a => a.TypeId).OnDelete(DeleteBehavior.Cascade);
            base.Configure(builder);
        }
    }
}
