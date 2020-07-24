using System;
using System.Collections.Generic;
using System.Text;
using HXCloud.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HXCloud.Repository.Maps
{
    public class ProjectModelMap : BaseModelMap<ProjectModel>
    {
        public override void Configure(EntityTypeBuilder<ProjectModel> builder)
        {
            builder.ToTable("Project").HasKey(a => a.Id);

            builder.HasOne(a => a.Group).WithMany(a => a.Projects).HasForeignKey(a => a.GroupId).OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(a => a.Parent).WithMany(a => a.Child).HasForeignKey(a => a.ParentId);//.IsRequired(false).OnDelete(DeleteBehavior.ClientSetNull);
            base.Configure(builder);
        }
    }
}
