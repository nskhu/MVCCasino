using BsMerchantAPI.Data.Repositories;

namespace BsMerchantAPI.Services;

public class AuthService(
    IAuthRepository authRepository) : IAuthService
{
    public string GeneratePrivateToken(string publicToken)
    {
        return authRepository.GeneratePrivateToken(publicToken);
    }
}