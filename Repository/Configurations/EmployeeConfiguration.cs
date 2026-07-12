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
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.Property(e => e.FullName)
               .IsRequired()
               .HasMaxLength(250);

            builder.Property(e => e.NationalID)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(e => e.Gender)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(e => e.Address)
               .IsRequired()
               .HasMaxLength(150);

            builder.Property(e => e.Position)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(e => e.Salary)
               .IsRequired()
               .HasColumnType("decimal(16,2)");

            ////////////////////////////////////////////////////////////////
            builder.HasOne(e => e.Branch)
                   .WithMany(e => e.Employees)
                   .HasForeignKey(e => e.BranchID);

            builder.HasOne(e=>e.User)
                   .WithOne(e=>e.Employee)
                   .HasForeignKey<Employee>(e => e.UserID);
        }
    }
}

