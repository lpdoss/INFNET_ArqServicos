using ProductService.Domain;
using Shared.Repository;

namespace ProductService.Repositories;

public interface IProductRepository : IRepository<Product>
{
    
}