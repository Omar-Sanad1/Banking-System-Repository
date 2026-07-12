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
    public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.Property(t => t.TransactionNumber)
                  .IsRequired()
                  .HasMaxLength(200);

            builder.Property(t => t.TransactionType)
                  .IsRequired()
                  .HasMaxLength(100);

            builder.Property(t => t.TransactionDescription)
                  .IsRequired()
                  .HasMaxLength(250);

            builder.Property(t => t.TransactionStatus)
                  .IsRequired()
                  .HasMaxLength(100);

            builder.Property(t => t.ReferenceNumber)
                  .IsRequired()
                  .HasMaxLength(200);

            builder.Property(t => t.TransactionAmount)
                  .IsRequired()
                  .HasColumnType("decimal(16,2)");

            //////////////////////////////////////////////////////////////////////////////
            builder.HasOne(t => t.Account)
                   .WithMany(t => t.Transactions)
                   .HasForeignKey(t => t.AccountID)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
