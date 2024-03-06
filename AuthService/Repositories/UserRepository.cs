using AuthService.Domain;
using Shared.Repository;

namespace AuthService.Repositories;

public class UserRepository : UnitOfWork<User>, IUserRepository
{
    public UserRepository(AuthDbContext context) : base(context)
    {
        
    }
}