using System.Data;
using Dapper;


namespace BsMerchantAPI.Data.Repositories.Dapper;

public class WalletRepositoryDapper(IDbConnection dbConnection) : IWalletRepository
{
    public decimal GetUserBalance(string userId)
    {
        var getByUserIdQuery = "SELECT [CurrentBalance] FROM Wallets WHERE UserId = @UserId";
        var result = dbConnection.QueryFirstOrDefault<decimal>(getByUserIdQuery, new { UserId = userId });
        return result;
    }
}