using System;
using System.Collections.Generic;
using System.Text;
using HXCloud.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HXCloud.Repository.Maps
{
    public class TypeDataDefineModelMap : BaseModelMap<TypeDataDefineModel>
    {
        public override void Configure(EntityTypeBuilder<TypeDataDefineModel> builder)
        {
            builder.ToTable("TypeDataDefine").HasKey(a => a.Id);
            builder.Property(a => a.Model).HasDefaultValue(DataDefineModel.W);
            builder.HasOne(a => a.Type).WithMany(a => a.TypeDataDefine).HasForeignKey(a => a.TypeId).OnDelete(DeleteBehavior.Cascade);
            base.Configure(builder);
        }

    }
}
