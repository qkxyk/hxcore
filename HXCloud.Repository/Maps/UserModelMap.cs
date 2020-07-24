using System;
using System.Collections.Generic;
using System.Text;
using HXCloud.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HXCloud.Repository.Maps
{
    public class UserModelMap : BaseModelMap<UserModel>
    {
        public override void Configure(EntityTypeBuilder<UserModel> builder)
        {
            builder.ToTable("User").HasKey(a => a.Id);
            builder.Property(a => a.LastLogin).ValueGeneratedOnAddOrUpdate();
            builder.HasOne(a => a.Group).WithMany(a => a.Users).HasForeignKey(a => a.GroupId).OnDelete(DeleteBehavior.Cascade);
            base.Configure(builder);
        }
    }
}
