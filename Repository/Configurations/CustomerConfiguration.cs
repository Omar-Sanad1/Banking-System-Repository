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
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.Property(c => c.FullName)
                .IsRequired()
                .HasMaxLength(250);

            builder.Property(c => c.NationalID)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(c => c.Gender)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(c => c.ResidintialAddress)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(c => c.Occuption)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(c => c.AccountStatus)
                .IsRequired()
                .HasMaxLength(100);

            //////////////////////////////////////////////////////////////////
            builder.HasOne(c => c.User)
                   .WithOne(c => c.Customer)
                   .HasForeignKey<Customer>(c => c.UserID)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(c => c.Accounts)
                   .WithOne(c => c.Customer)
                   .HasForeignKey(c => c.CustomerID)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(c => c.Loans)
                   .WithOne(c => c.Customer)
                   .HasForeignKey(c => c.CustomerID)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
