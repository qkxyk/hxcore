using System;
using System.Collections.Generic;
using System.Text;
using HXCloud.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HXCloud.Repository.Maps
{
    public class TypeModuleModelMap : BaseModelMap<TypeModuleModel>
    {
        public override void Configure(EntityTypeBuilder<TypeModuleModel> builder)
        {
            builder.ToTable("TypeModule").HasKey(a => a.Id);
            builder.HasOne(a => a.Type).WithMany(a => a.TypeModules).HasForeignKey(a => a.TypeId).OnDelete(DeleteBehavior.Cascade);
            base.Configure(builder);
        }
    }
}
