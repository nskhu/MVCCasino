namespace MVCCasino.Services;

public interface IAuthService
{
    string GeneratePublicToken(string userId);
}