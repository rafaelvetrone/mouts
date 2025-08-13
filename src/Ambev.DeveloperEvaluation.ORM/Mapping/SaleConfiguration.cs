using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.RegularExpressions;

namespace Ambev.DeveloperEvaluation.ORM.Mapping;

public class SaleConfiguration : IEntityTypeConfiguration<Sale>
{
    public void Configure(EntityTypeBuilder<Sale> builder)
    {
        builder.ToTable("Sales");

        builder.HasKey(u => u.Id);
        builder.Property(u => u.Id).HasColumnType("uuid").HasDefaultValueSql("gen_random_uuid()");

        // Properties
        builder.Property(s => s.SaleNumber)
               .IsRequired()
               .HasMaxLength(50);

        builder.Property(s => s.SaleDate)
               .IsRequired()
               .HasColumnType("timestamp with time zone");

        builder.Property(s => s.CustomerId)
               .HasMaxLength(100);

        builder.Property(s => s.CustomerName)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(s => s.CustomerEmail)
               .IsRequired()
               .HasMaxLength(200);

        builder.Property(s => s.Branch)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(s => s.TotalAmount)
               .IsRequired()
               .HasColumnType("decimal(18,2)");

        builder.Property(s => s.IsCancelled)
               .IsRequired()
               .HasDefaultValue(false);

        // Relationships
        builder.HasMany(s => s.Items)
               .WithOne(i => i.Sale)
               .HasForeignKey(i => i.SaleId)
               .OnDelete(DeleteBehavior.Cascade)
               .Metadata.PrincipalToDependent.SetField("_items");

        // Optional: Indexes
        builder.HasIndex(s => s.SaleNumber).IsUnique();
    }
}
