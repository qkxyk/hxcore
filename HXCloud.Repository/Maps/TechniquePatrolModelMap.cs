using HXCloud.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.Repository.Maps
{
    public class TechniquePatrolModelMap : IEntityTypeConfiguration<TechniquePatrolModel>
    {
        public void Configure(EntityTypeBuilder<TechniquePatrolModel> builder)
        {
            builder.ToTable("TechniquePatrol").HasKey(a => a.PatrolId);
            //builder.HasOne(a => a.PatrolData).WithOne(a => a.TechniquePatrol).HasForeignKey<TechniquePatrolModel>(a => a.PatrolId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
