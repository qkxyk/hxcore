using HXCloud.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.Repository.Maps
{
    public class IssueModelMap: BaseModelMap<IssueModel>
    {
        public override void Configure(EntityTypeBuilder<IssueModel> builder)
        {
            //builder.Property(a => a.EditTime).ValueGeneratedOnAddOrUpdate();
            //builder.Property(a => a.CreateTime).HasDefaultValueSql("getdate()");
            //builder.ToTable("Group").HasKey(a => a.Id);
            builder.ToTable("Issue").HasKey(a => a.Id);
            base.Configure(builder);
        }
    }
}
