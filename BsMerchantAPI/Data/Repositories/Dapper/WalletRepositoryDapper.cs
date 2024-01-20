using System.Data;
using BsMerchantAPI.Models.Responses.ResponseDatas;
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

    public PlayerInfoData GetPlayerInfo(string privateToken)
    {
        const string procedureName = "GetPlayerInfoProcedure";
        var parameters = new { PrivateToken = privateToken };
        var results = dbConnection
            .Query<PlayerInfoData>(procedureName, parameters, commandType: CommandType.StoredProcedure)
            .SingleOrDefault();

        return results;
    }
}