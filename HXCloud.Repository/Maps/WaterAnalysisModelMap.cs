using HXCloud.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.Repository.Maps
{
    public class WaterAnalysisModelMap : IEntityTypeConfiguration<WaterAnalysisModel>
    {
        public void Configure(EntityTypeBuilder<WaterAnalysisModel> builder)
        {
            builder.ToTable("WaterAnalysis").HasKey(a => a.PatrolId);
            //builder.HasOne(a => a.PatrolData).WithOne(a => a.WaterAnalysis).HasForeignKey<WaterAnalysisModel>(a => a.PatrolId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
