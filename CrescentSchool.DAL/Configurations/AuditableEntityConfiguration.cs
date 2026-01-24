using CrescentSchool.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrescentSchool.DAL.Configurations;

public class AuditableEntityConfiguration<T> : IEntityTypeConfiguration<T> where T : AuditableEntity
{
    public void Configure(EntityTypeBuilder<T> builder)
    {
        builder.Property(e => e.CreatedAt)
            .IsRequired(false);

        builder.Property(e => e.UpdatedAt)
            .IsRequired(false);

        builder.Property(e => e.DeletedAt)
            .IsRequired(false);
    }
}
