using System;
using System.Collections.Generic;
using System.Text;
using HXCloud.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HXCloud.Repository.Maps
{
    public class DeviceModelMap : BaseModelMap<DeviceModel>
    {
        public override void Configure(EntityTypeBuilder<DeviceModel> builder)
        {
            builder.ToTable("Device").HasKey(a => a.DeviceSn);
            builder.HasOne(a => a.Group).WithMany(a => a.Devices).HasForeignKey(a => a.GroupId).OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(a => a.Project).WithMany(a => a.Devices).HasForeignKey(a => a.ProjectId).IsRequired(false).OnDelete(DeleteBehavior.ClientSetNull);
            builder.HasOne(a => a.Type).WithMany(a => a.Devices).HasForeignKey(a => a.TypeId).OnDelete(DeleteBehavior.Restrict);
            base.Configure(builder);
        }
    }
}
