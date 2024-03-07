using PaymentService.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PaymentService.Repositories.Mappings;

public class OrderMapping : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Order");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).IsRequired().HasColumnName("Id");
        builder.Property(x => x.UserId).IsRequired().HasColumnName("UserId");
        builder.Property(x => x.PaymentMethodId).IsRequired().HasColumnName("PaymentMethodId");
        builder.Property(x => x.StatusId).IsRequired().HasColumnName("StatusId");
    }
}