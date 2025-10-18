using Shop.Api.Models;

namespace Shop.Api.Interfaces;

public interface IJwtService
{
    Task<string> GenerateToken(ApplicationUser user);

}
