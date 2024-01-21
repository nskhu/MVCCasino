using System.Data;
using BsMerchantAPI.Models.Responses.ResponseDatas;
using Dapper;
using Microsoft.Data.SqlClient;


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
        var result = dbConnection
            .Query<PlayerInfoData>(procedureName, parameters, commandType: CommandType.StoredProcedure)
            .SingleOrDefault();

        return result;
    }

    public BetResponseData AddBetTransaction(string remoteTransactionId, decimal amount, string privateToken)
    {
        var parameters = new
        {
            RemoteTransactionId = remoteTransactionId,
            Amount = amount,
            PrivateToken = privateToken
        };
        int transactionId;
        decimal currentBalance;

        try
        {
            var result = dbConnection.QuerySingle<dynamic>(
                "AddBetTransactionProcedure",
                parameters,
                commandType: CommandType.StoredProcedure
            );
            transactionId = (int)result.TransactionId;
            currentBalance = (decimal)result.NewCurrentBalance;
        }
        catch (SqlException e)
        {
            Console.WriteLine(e.Message);
            Console.WriteLine(e.Number);
            throw;
        }

        return new BetResponseData
        {
            TransactionId = transactionId,
            CurrentBalance = currentBalance
        };
    }

    public BetResponseData? AddWinTransaction(string remoteTransactionId, decimal amount, string privateToken)
    {
        var parameters = new
        {
            RemoteTransactionId = remoteTransactionId,
            Amount = amount,
            PrivateToken = privateToken
        };
        int transactionId;
        decimal currentBalance;

        try
        {
            var result = dbConnection.QuerySingle<dynamic>(
                "AddWinTransactionProcedure",
                parameters,
                commandType: CommandType.StoredProcedure
            );
            transactionId = (int)result.TransactionId;
            currentBalance = (decimal)result.NewCurrentBalance;
        }
        catch (SqlException e)
        {
            Console.WriteLine(e.Message);
            Console.WriteLine(e.Number);
            throw;
        }

        return new BetResponseData
        {
            TransactionId = transactionId,
            CurrentBalance = currentBalance
        };
    }
}