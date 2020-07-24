using System;
using System.Collections.Generic;
using System.Text;
using HXCloud.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HXCloud.Repository.Maps
{
    public class DeviceCardModelMap : BaseModelMap<DeviceCardModel>
    {
        public override void Configure(EntityTypeBuilder<DeviceCardModel> builder)
        {
            builder.ToTable("DeviceCard").HasKey(a => a.CardNo);
            builder.HasOne(a => a.Device).WithOne(a => a.DeviceCard).HasForeignKey<DeviceCardModel>(a => a.DeviceSn).OnDelete(DeleteBehavior.Cascade);
            base.Configure(builder);
        }
    }
}
