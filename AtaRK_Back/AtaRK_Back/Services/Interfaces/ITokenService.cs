using AtaRK.Models;

namespace AtaRK.Services
{
    public interface ITokenService
    {
        string CreateToken(User user);
    }
}
