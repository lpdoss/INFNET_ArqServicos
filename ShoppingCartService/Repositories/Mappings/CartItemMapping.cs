using ShoppingCartService.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ShoppingCartService.Repositories.Mappings;

public class CartItemMapping : IEntityTypeConfiguration<CartItem>
{
    public void Configure(EntityTypeBuilder<CartItem> builder)
    {
        builder.ToTable("CartItem");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).IsRequired().HasColumnName("Id");
        builder.Property(x => x.CartId).IsRequired().HasColumnName("CartId");
        builder.Property(x => x.ProductId).IsRequired().HasColumnName("ProductId");
        builder.Property(x => x.Amount).IsRequired().HasColumnName("Amount");
        builder.Property(x => x.IncludeInOrder).IsRequired().HasColumnName("IncludeInOrder");
    }
}