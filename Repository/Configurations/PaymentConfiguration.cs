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
    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.Property(p => p.PaymentMethod)
                  .IsRequired()
                  .HasMaxLength(100);

            builder.Property(p => p.PaymentStatus)
                  .IsRequired()
                  .HasMaxLength(100);

            builder.Property(p => p.ReferenceNumber)
                  .IsRequired()
                  .HasMaxLength(200);

            builder.Property(p => p.ServiceType)
                  .IsRequired()
                  .HasMaxLength(100);

            builder.Property(p => p.PaymentAmount)
                  .IsRequired()
                  .HasColumnType("decimal(16,2)");
            ///////////////////////////////////////////////////////////////////
            builder.HasOne(p => p.Account)
                   .WithMany(p => p.Payments)
                   .HasForeignKey(p => p.AccountID)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
