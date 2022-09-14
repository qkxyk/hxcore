using HXCloud.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.Repository.Maps
{
    public class DevicePatrolModelMap : IEntityTypeConfiguration<DevicePatrolModel>
    {
        public void Configure(EntityTypeBuilder<DevicePatrolModel> builder)
        {
            builder.ToTable("DevicePatrol").HasKey(a => a.PatrolId);
            //builder.HasOne(a => a.PatrolData).WithOne(a => a.DevicePatrol).HasForeignKey<DevicePatrolModel>(a => a.PatrolId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
