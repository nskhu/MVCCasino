namespace BsMerchantAPI.Data.Repositories;

public interface IAuthRepository
{
    string GeneratePrivateToken(string publicToken);
}