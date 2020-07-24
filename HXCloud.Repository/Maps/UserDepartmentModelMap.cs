using HXCloud.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.Repository.Maps
{
    public class UserDepartmentModelMap : BaseModelMap<UserDepartmentModel>
    {
        public override void Configure(EntityTypeBuilder<UserDepartmentModel> builder)
        {
            builder.ToTable("UserDepartment").HasKey(a => new { a.UserId, a.DeparmentId });
            builder.HasOne(a => a.User).WithMany(a => a.UserDepartments).HasForeignKey(a => a.UserId).OnDelete(DeleteBehavior.ClientSetNull);
            builder.HasOne(a => a.Department).WithMany(a => a.UserDepartments).HasForeignKey(a => a.DeparmentId).OnDelete(DeleteBehavior.Cascade);
            base.Configure(builder);
        }
    }
}
