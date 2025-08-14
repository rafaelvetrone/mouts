using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping;

public class SaleItemConfiguration : IEntityTypeConfiguration<SaleItem>
{
    public void Configure(EntityTypeBuilder<SaleItem> builder)
    {
        // Table
        builder.ToTable("SaleItems");

        // Primary Key
        builder.HasKey(si => si.Id);
        builder.Property(si => si.Id)
               .HasColumnType("uuid")
               .HasDefaultValueSql("gen_random_uuid()");

        // Properties
        builder.Property(si => si.ProductId)
               .IsRequired();

        builder.Property(si => si.ProductName)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(si => si.Quantity)
               .IsRequired();

        builder.Property(si => si.UnitPrice)
               .IsRequired()
               .HasColumnType("decimal(18,2)");

        builder.Property(si => si.Discount)
               .HasColumnType("decimal(18,2)")
               .HasDefaultValue(0);

        builder.Property(si => si.TotalAmount)
               .IsRequired()
               .HasColumnType("decimal(18,2)");

        // Relationships
        builder.HasOne(si => si.Sale)
               .WithMany(s => s.Items)
               .HasForeignKey(si => si.SaleId)
               .OnDelete(DeleteBehavior.Cascade);

        // Optional: Indexes
        builder.HasIndex(si => si.SaleId);
    }
}
