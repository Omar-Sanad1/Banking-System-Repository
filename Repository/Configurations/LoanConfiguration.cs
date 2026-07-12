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
    public class LoanConfiguration : IEntityTypeConfiguration<Loan>
    {
        public void Configure(EntityTypeBuilder<Loan> builder)
        {
            builder.Property(l => l.LoanType)
                  .IsRequired()
                  .HasMaxLength(100);

            builder.Property(l => l.ApprovalStatus)
                  .IsRequired()
                  .HasMaxLength(100);

            builder.Property(l => l.RequestedAmount)
                  .IsRequired()
                  .HasColumnType("decimal(16,2)");

            builder.Property(l => l.InterestRate)
                  .IsRequired()
                  .HasColumnType("decimal(16,2)");

            builder.Property(l => l.MonthlyInstallmentAmount)
                  .IsRequired()
                  .HasColumnType("decimal(16,2)");
        }
    }
}
