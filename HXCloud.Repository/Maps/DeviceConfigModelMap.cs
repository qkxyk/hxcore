using System;
using System.Collections.Generic;
using System.Text;
using HXCloud.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HXCloud.Repository.Maps
{
    public class DeviceConfigModelMap : BaseModelMap<DeviceConfigModel>
    {
        public override void Configure(EntityTypeBuilder<DeviceConfigModel> builder)
        {
            builder.ToTable("DeviceConfig").HasKey(a => a.Id);
            builder.HasOne(a => a.Device).WithMany(a => a.DeviceConfig).HasForeignKey(a => a.DeviceSn).OnDelete(DeleteBehavior.Cascade);
            base.Configure(builder);
        }
    }
}
