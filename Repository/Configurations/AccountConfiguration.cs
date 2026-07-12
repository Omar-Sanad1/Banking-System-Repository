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
    public class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.Property(a => a.AccountNumber)
                   .IsRequired()
                   .HasMaxLength(150);

            builder.Property(a => a.AccountType)
                   .IsRequired()
                   .HasMaxLength(150);

            builder.Property(a => a.Currency)
                  .IsRequired()
                  .HasMaxLength(50);

            builder.Property(a => a.CurrentBalance)
                   .IsRequired()
                   .HasColumnType("decimal(16,2)");

            builder.Property(a => a.MinimumRequiredBalance)
                  .IsRequired()
                  .HasColumnType("decimal(16,2)");

        }
    }
}
