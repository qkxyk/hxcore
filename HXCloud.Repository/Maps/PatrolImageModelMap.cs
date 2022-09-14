using HXCloud.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.Repository.Maps
{
    public class PatrolImageModelMap: IEntityTypeConfiguration<PatrolImageModel>
    {
        public virtual void Configure(EntityTypeBuilder<PatrolImageModel> builder)
        {
            builder.ToTable("PatrolImage").HasKey(a => a.PatrolId);
            //builder.HasOne(a => a.PatrolData).WithOne(a => a.PatrolImage).HasForeignKey<PatrolImageModel>(a => a.PatrolId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
