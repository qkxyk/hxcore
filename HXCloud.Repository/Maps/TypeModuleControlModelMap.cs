using System;
using System.Collections.Generic;
using System.Text;
using HXCloud.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HXCloud.Repository.Maps
{
    public class TypeModuleControlModelMap : BaseModelMap<TypeModuleControlModel>
    {
        public override void Configure(EntityTypeBuilder<TypeModuleControlModel> builder)
        {
            builder.ToTable("TypeModuleControl").HasKey(a => a.Id);
            builder.HasOne(a => a.TypeModule).WithMany(a => a.ModuleControls).HasForeignKey(a => a.ModuleId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(a => a.TypeDataDefine).WithMany(a => a.TypeModuleControls).HasForeignKey(a => a.DataDefineId).OnDelete(DeleteBehavior.ClientSetNull);
            builder.HasOne(a => a.TypeClass).WithMany(a => a.TypeModuleControls).HasForeignKey(a => a.ClassId).OnDelete(DeleteBehavior.ClientSetNull);
            base.Configure(builder);
        }
    }
}
