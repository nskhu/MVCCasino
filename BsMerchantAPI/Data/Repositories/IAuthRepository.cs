namespace BsMerchantAPI.Data.Repositories;

public interface IAuthRepository
{
    (string? PrivateToken, int StatusCode) GeneratePrivateToken(string publicToken);
}