using ProductCrudApi.Domain.Entities;

namespace ProductCrudApi.Application.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}