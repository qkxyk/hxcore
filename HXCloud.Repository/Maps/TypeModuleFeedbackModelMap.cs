using System;
using System.Collections.Generic;
using System.Text;
using HXCloud.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HXCloud.Repository.Maps
{
    public class TypeModuleFeedbackModelMap : BaseModelMap<TypeModuleFeedbackModel>
    {
        public override void Configure(EntityTypeBuilder<TypeModuleFeedbackModel> builder)
        {
            builder.ToTable("TypeModuleFeedback").HasKey(a => a.Id);
            builder.HasOne(a => a.TypeModuleControl).WithMany(a => a.TypeModuleFeedbacks).HasForeignKey(a => a.ModuleControlId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(a => a.TypeDataDefine).WithMany(a => a.TypeModuleFeedbacks).HasForeignKey(a => a.DataDefineId).OnDelete(DeleteBehavior.ClientSetNull);
            base.Configure(builder);
        }
    }
}
