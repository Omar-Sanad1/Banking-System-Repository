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
    public class CardConfiguration : IEntityTypeConfiguration<Card>
    {
        public void Configure(EntityTypeBuilder<Card> builder)
        {
            builder.Property(c => c.CardNumber)
                 .IsRequired()
                 .HasMaxLength(250);

            builder.Property(c => c.CardType)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(c => c.CardStatus)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(c => c.DailyTransactionLimit)
                .IsRequired()
                .HasColumnType("decimal(16,2)");

            //////////////////////////////////////////////////////////
            builder.HasOne(c => c.Account)
                   .WithMany(c => c.Cards)
                   .HasForeignKey(c => c.AccountID)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
