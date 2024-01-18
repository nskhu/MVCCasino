using MVCCasino.Data.Repository;

namespace MVCCasino.Services;

public class AuthService(IAuthRepository authRepository) : IAuthService
{
    public string GeneratePublicToken(string userId)
    {
        return authRepository.GeneratePublicToken(userId);
    }
}