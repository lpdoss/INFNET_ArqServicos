using ProductService.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ProductService.Repositories.Mappings;

public class ProductMapping : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Product");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).IsRequired().HasColumnName("Id");
        builder.Property(x => x.Name).IsRequired().HasColumnName("Name");
        builder.Property(x => x.Description).IsRequired().HasColumnName("Description");
        builder.Property(x => x.Price).IsRequired().HasColumnName("Price");
        builder.Property(x => x.AmountInStock).IsRequired().HasColumnName("AmountInStock");
    }
}