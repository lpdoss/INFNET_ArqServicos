using PaymentService.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PaymentService.Repositories.Mappings;

public class PaymentMethodMapping : IEntityTypeConfiguration<PaymentMethod>
{
    public void Configure(EntityTypeBuilder<PaymentMethod> builder)
    {
        builder.ToTable("PaymentMethod");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).IsRequired().HasColumnName("Id");
        builder.Property(x => x.Name).IsRequired().HasColumnName("Name");
    }
}