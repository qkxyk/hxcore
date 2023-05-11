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
            //builder.HasOne(a => a.Repair).WithOne(a => a.Issue).HasForeignKey<RepairModel>(a => a.IssueId);//配置一个问题单只能处理一次
            base.Configure(builder);
        }
    }
}
