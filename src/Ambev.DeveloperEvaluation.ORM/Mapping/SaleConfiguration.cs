﻿using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping
{
    public class SaleConfiguration : IEntityTypeConfiguration<Sale>
    {
        public void Configure(EntityTypeBuilder<Sale> builder)
        {
            builder.ToTable("Sales");

            builder.HasKey(s => s.Id);
            builder.Property(s => s.Id).HasColumnType("uuid").HasDefaultValueSql("gen_random_uuid()");

            builder.Property(s => s.SaleNumber).IsRequired().HasMaxLength(50);
            builder.Property(s => s.CreatedDate).IsRequired().HasColumnType("timestamp without time zone");
            builder.Property(s => s.UpdatedDate).HasColumnType("timestamp without time zone");
            builder.Property(s => s.CustomerId).IsRequired();
            builder.Property(s => s.CustomerName).IsRequired().HasMaxLength(50);
            builder.Property(s => s.TotalAmount).IsRequired();
            builder.Property(s => s.BranchId).IsRequired();
            builder.Property(s => s.BranchName).IsRequired().HasMaxLength(250);            
            builder.Property(s => s.IsCancelled).IsRequired();

            builder.HasMany(s => s.Items)
                .WithOne()  
                .HasForeignKey(i => i.SaleId)  
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
