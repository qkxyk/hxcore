using HXCloud.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.Repository.Maps
{
    public class PatrolDataModelMap : BaseModelMap<PatrolDataModel>
    {
        public override void Configure(EntityTypeBuilder<PatrolDataModel> builder)
        {
            builder.ToTable("PatrolData").HasKey(a => a.Id);

            builder.HasOne(a => a.PatrolImage).WithOne(a => a.PatrolData).HasForeignKey<PatrolImageModel>(a => a.PatrolId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(a => a.ProductData).WithOne(a => a.PatrolData).HasForeignKey<ProductDataModel>(a => a.PatrolId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(a => a.WaterAnalysis).WithOne(a => a.PatrolData).HasForeignKey<WaterAnalysisModel>(a => a.PatrolId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(a => a.DevicePatrol).WithOne(a => a.PatrolData).HasForeignKey<DevicePatrolModel>(a => a.PatrolId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(a => a.TechniquePatrol).WithOne(a => a.PatrolData).HasForeignKey<TechniquePatrolModel>(a => a.PatrolId).OnDelete(DeleteBehavior.Cascade);

            base.Configure(builder);
        }
    }
}
