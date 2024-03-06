using ProductService.Domain;
using Shared.Repository;

namespace ProductService.Repositories;

public class ProductRepository : UnitOfWork<Product>, IProductRepository
{
    public ProductRepository(ProductDbContext context) : base(context)
    {
        
    }
}