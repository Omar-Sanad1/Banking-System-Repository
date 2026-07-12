using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.Property(r => r.RoleName)
                  .IsRequired()
                  .HasMaxLength(100);

            ////////////////////////////////////////////////////////////////
            builder.HasMany(r => r.Users)
                   .WithOne(r => r.Role)
                   .HasForeignKey(r => r.RoleID)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
