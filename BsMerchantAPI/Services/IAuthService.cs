namespace BsMerchantAPI.Services;

public interface IAuthService
{
    string GeneratePrivateToken(string publicToken);
}