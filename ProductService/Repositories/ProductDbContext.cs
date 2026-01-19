using Microsoft.EntityFrameworkCore;

namespace ProductService.Repositories;

public class ProductDbContext : DbContext
{
    public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options)
    {

        bool isFrozen = options.IsFrozen;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProductDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        bool isConfigured = optionsBuilder.IsConfigured;
        base.OnConfiguring(optionsBuilder);
    }
}