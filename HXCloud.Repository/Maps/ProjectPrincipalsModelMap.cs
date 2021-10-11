using System;
using System.Collections.Generic;
using System.Text;
using HXCloud.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HXCloud.Repository.Maps
{
    public class ProjectPrincipalsModelMap : BaseModelMap<ProjectPrincipalsModel>
    {
        public override void Configure(EntityTypeBuilder<ProjectPrincipalsModel> builder)
        {
            builder.ToTable("ProjectPrincipals").HasKey(a => a.Id);
            builder.HasOne(a => a.Project).WithMany(a => a.Principals).HasForeignKey(a => a.ProjectId).OnDelete(DeleteBehavior.Cascade);
            base.Configure(builder);
        }
    }
}
