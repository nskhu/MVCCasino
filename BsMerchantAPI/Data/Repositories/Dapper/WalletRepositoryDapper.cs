using System.Data;
using Dapper;


namespace BsMerchantAPI.Data.Repositories.Dapper;

public class WalletRepositoryDapper(IDbConnection dbConnection) : IWalletRepository
{
    public decimal GetBalance(string privateToken)
    {
        const string procedureName = "GetBalanceProcedure";
        var parameters = new { PrivateToken = privateToken };
        var balance =
            dbConnection.QueryFirstOrDefault<decimal>(procedureName, parameters,
                commandType: CommandType.StoredProcedure);

        return balance;
    }
}