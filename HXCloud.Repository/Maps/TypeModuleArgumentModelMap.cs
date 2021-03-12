using HXCloud.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HXCloud.Repository.Maps
{
    public class TypeModuleArgumentModelMap : BaseModelMap<TypeModuleArgumentModel>
    {
        public override void Configure(EntityTypeBuilder<TypeModuleArgumentModel> builder)
        {
            builder.ToTable("TypeModuleArgument").HasKey(a => a.Id);
            builder.HasOne(a => a.TypeModule).WithMany(a => a.ModeleArguments).HasForeignKey(a => a.ModuleId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(a => a.TypeDataDefine).WithMany(a => a.TypeModuleArguments).HasForeignKey(a => a.DataDefineId).OnDelete(DeleteBehavior.ClientSetNull);
            base.Configure(builder);
        }
    }
}
