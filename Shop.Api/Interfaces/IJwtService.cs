using Shop.Api.Models;

namespace Shop.Api.Interfaces;

public interface IJwtService
{
    string GenerateToken(ApplicationUser user);

}
