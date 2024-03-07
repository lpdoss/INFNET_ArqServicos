using PaymentService.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PaymentService.Repositories.Mappings;

public class OrderItemMapping : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable("OrderItem");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).IsRequired().HasColumnName("Id");
        builder.Property(x => x.OrderId).IsRequired().HasColumnName("OrderId");
        builder.Property(x => x.ProductId).IsRequired().HasColumnName("ProductId");
        builder.Property(x => x.Amount).IsRequired().HasColumnName("Amount");
        builder.Property(x => x.Price).IsRequired().HasColumnName("Price");
    }
}