using System;
using System.Collections.Generic;
using System.Text;
using HXCloud.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HXCloud.Repository.Maps
{
    public class ProjectImageModelMap : BaseModelMap<ProjectImageModel>
    {
        public override void Configure(EntityTypeBuilder<ProjectImageModel> builder)
        {
            builder.ToTable("ProjectImage").HasKey(a => a.Id);
            builder.HasOne(a => a.project).WithMany(a => a.Images).HasForeignKey(p => p.ProjectId).OnDelete(DeleteBehavior.Cascade);
            base.Configure(builder);
        }
    }
}
