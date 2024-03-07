using PaymentService.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PaymentService.Repositories.Mappings;

public class PaymentMapping : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.ToTable("Payment");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).IsRequired().HasColumnName("Id");
        builder.Property(x => x.OrderId).IsRequired().HasColumnName("OrderId");
        builder.Property(x => x.Financial).IsRequired().HasColumnName("Financial");
    }
}