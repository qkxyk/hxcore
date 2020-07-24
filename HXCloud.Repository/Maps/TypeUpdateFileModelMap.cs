using System;
using System.Collections.Generic;
using System.Text;
using HXCloud.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HXCloud.Repository.Maps
{
    public class TypeUpdateFileModelMap : BaseModelMap<TypeUpdateFileModel>
    {
        public override void Configure(EntityTypeBuilder<TypeUpdateFileModel> builder)
        {
            builder.ToTable("TypeUpdateFile").HasKey(a => a.Id);
            builder.HasOne(a => a.Type).WithMany(a => a.TypeUpdateFiles).HasForeignKey(a => a.TypeId).OnDelete(DeleteBehavior.Cascade);
            base.Configure(builder);
        }
    }
}
