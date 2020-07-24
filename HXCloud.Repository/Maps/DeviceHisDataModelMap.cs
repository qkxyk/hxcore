using System;
using System.Collections.Generic;
using System.Text;
using HXCloud.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HXCloud.Repository.Maps
{
  public  class DeviceHisDataModelMap:BaseModelMap<DeviceHisDataModel>
    {
        public override void Configure(EntityTypeBuilder<DeviceHisDataModel> builder)
        {
            builder.ToTable("DeviceHisData").HasKey(a => a.Id);
            builder.HasOne(a => a.Device).WithMany(a => a.DeviceHisData).HasForeignKey(a => a.DeviceSn).OnDelete(DeleteBehavior.Cascade);
            base.Configure(builder);    
        }
    }
}
