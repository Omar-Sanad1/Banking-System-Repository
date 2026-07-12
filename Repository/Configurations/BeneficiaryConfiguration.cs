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
    public class BeneficiaryConfiguration : IEntityTypeConfiguration<Benificiary>
    {
        public void Configure(EntityTypeBuilder<Benificiary> builder)
        {
            builder.Property(b => b.BenificiaryName)
                   .IsRequired()
                   .HasMaxLength(250);

            builder.Property(b => b.AccountNumber)
                  .IsRequired()
                  .HasMaxLength(250);

            builder.Property(b => b.BankName)
                  .IsRequired()
                  .HasMaxLength(150);

            builder.Property(b => b.RelationshipType)
                  .IsRequired()
                  .HasMaxLength(100);

            builder.Property(b => b.VerificationStatus)
                 .IsRequired()
                 .HasMaxLength(100);


            /////////////////////////////////////////////////////////////////////////
            builder.HasOne(b => b.Account)
                   .WithMany(b => b.Benificiaries)
                   .HasForeignKey(b => b.AccountID);


        }
    }
}
