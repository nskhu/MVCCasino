using System.Data;
using BsMerchantAPI.Models.Responses.ResponseDatas;
using Dapper;
using Microsoft.Data.SqlClient;


namespace BsMerchantAPI.Data.Repositories.Dapper;

public class WalletRepositoryDapper(IDbConnection dbConnection) : IWalletRepository
{
    public (decimal Balance, int StatusCode) GetBalance(string privateToken)
    {
        const string procedureName = "GetBalanceProcedure";
        var parameters = new DynamicParameters();
        parameters.Add("@PrivateToken", privateToken, DbType.String, ParameterDirection.Input);
        parameters.Add("@Balance", dbType: DbType.Decimal, direction: ParameterDirection.Output);
        parameters.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);

        dbConnection.Execute(procedureName, parameters, commandType: CommandType.StoredProcedure);

        var balance = parameters.Get<decimal>("@Balance");
        var statusCode = parameters.Get<int>("@StatusCode");

        return (balance, statusCode);
    }

    public (PlayerInfoData PlayerInfo, int StatusCode) GetPlayerInfo(string privateToken)
    {
        const string procedureName = "GetPlayerInfoProcedure";
        var parameters = new DynamicParameters();
        parameters.Add("@PrivateToken", privateToken, DbType.String, ParameterDirection.Input);
        parameters.Add("@UserId", dbType: DbType.String, size: 100, direction: ParameterDirection.Output);
        parameters.Add("@UserName", dbType: DbType.String, size: 100, direction: ParameterDirection.Output);
        parameters.Add("@FirstName", dbType: DbType.String, size: 100, direction: ParameterDirection.Output);
        parameters.Add("@LastName", dbType: DbType.String, size: 100, direction: ParameterDirection.Output);
        parameters.Add("@Email", dbType: DbType.String, size: 100, direction: ParameterDirection.Output);
        parameters.Add("@CurrentBalance", dbType: DbType.Decimal, direction: ParameterDirection.Output);
        parameters.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);

        dbConnection.Execute(procedureName, parameters, commandType: CommandType.StoredProcedure);

        var statusCode = parameters.Get<int>("@StatusCode");

        if (statusCode != 200) return (null, statusCode);
        var playerInfo = new PlayerInfoData
        {
            UserId = parameters.Get<string>("@UserId"),
            UserName = parameters.Get<string>("@UserName"),
            FirstName = parameters.Get<string>("@FirstName"),
            LastName = parameters.Get<string>("@LastName"),
            Email = parameters.Get<string>("@Email"),
            CurrentBalance = parameters.Get<decimal>("@CurrentBalance")
        };

        return (playerInfo, statusCode);
    }

    public (BetResponseData BetResponse, int StatusCode) AddBetTransaction(string remoteTransactionId, decimal amount,
        string privateToken)
    {
        const string procedureName = "AddBetTransactionProcedure";
        var parameters = new DynamicParameters();
        parameters.Add("@RemoteTransactionId", remoteTransactionId, DbType.String, ParameterDirection.Input);
        parameters.Add("@Amount", amount, DbType.Decimal, ParameterDirection.Input);
        parameters.Add("@PrivateToken", privateToken, DbType.String, ParameterDirection.Input);
        parameters.Add("@TransactionId", dbType: DbType.Int32, direction: ParameterDirection.Output);
        parameters.Add("@NewCurrentBalance", dbType: DbType.Decimal, direction: ParameterDirection.Output);
        parameters.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);

        dbConnection.Execute(procedureName, parameters, commandType: CommandType.StoredProcedure);

        var statusCode = parameters.Get<int>("@StatusCode");

        if (statusCode != 200) return (null, statusCode);

        var transactionId = parameters.Get<int>("@TransactionId");
        var newCurrentBalance = parameters.Get<decimal>("@NewCurrentBalance");
        var betResponse = new BetResponseData
        {
            TransactionId = transactionId,
            CurrentBalance = newCurrentBalance
        };

        return (betResponse, statusCode);
    }

    public (BetResponseData BetResponse, int StatusCode) AddWinTransaction(string remoteTransactionId, decimal amount,
        string privateToken)
    {
        const string procedureName = "AddWinTransactionProcedure";
        var parameters = new DynamicParameters();
        parameters.Add("@RemoteTransactionId", remoteTransactionId, DbType.String, ParameterDirection.Input);
        parameters.Add("@Amount", amount, DbType.Decimal, ParameterDirection.Input);
        parameters.Add("@PrivateToken", privateToken, DbType.String, ParameterDirection.Input);
        parameters.Add("@TransactionId", dbType: DbType.Int32, direction: ParameterDirection.Output);
        parameters.Add("@NewCurrentBalance", dbType: DbType.Decimal, direction: ParameterDirection.Output);
        parameters.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);

        dbConnection.Execute(procedureName, parameters, commandType: CommandType.StoredProcedure);

        var statusCode = parameters.Get<int>("@StatusCode");

        if (statusCode != 200) return (null, statusCode);

        var transactionId = parameters.Get<int>("@TransactionId");
        var newCurrentBalance = parameters.Get<decimal>("@NewCurrentBalance");

        var betResponse = new BetResponseData
        {
            TransactionId = transactionId,
            CurrentBalance = newCurrentBalance
        };

        return (betResponse, statusCode);
    }

    public (TransactionResponseData TransactionData, int StatusCode) AddCancelBetTransaction(
        string remoteTransactionId,
        decimal amount,
        string privateToken,
        string betTransactionId)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@RemoteTransactionId", remoteTransactionId, DbType.String, ParameterDirection.Input);
        parameters.Add("@Amount", amount, DbType.Decimal, ParameterDirection.Input);
        parameters.Add("@PrivateToken", privateToken, DbType.String, ParameterDirection.Input);
        parameters.Add("@BetTransactionId", betTransactionId, DbType.String, ParameterDirection.Input);
        parameters.Add("@RemoteTransactionIdOut", dbType: DbType.String, size: 100,
            direction: ParameterDirection.Output);
        parameters.Add("@NewCurrentBalance", dbType: DbType.Decimal, direction: ParameterDirection.Output);
        parameters.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);

        try
        {
            dbConnection.Execute(
                "AddCancelBetTransactionProcedure",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            var statusCode = parameters.Get<int>("@StatusCode");

            if (statusCode != 200)
            {
                return (null, statusCode);
            }

            var transactionData = new TransactionResponseData
            {
                TransactionId = parameters.Get<string>("@RemoteTransactionIdOut"),
                CurrentBalance = parameters.Get<decimal>("@NewCurrentBalance")
            };

            return (transactionData, statusCode);
        }
        catch (SqlException e)
        {
            Console.WriteLine(e.Message);
            Console.WriteLine(e.Number);
            throw;
        }
    }


    public (TransactionResponseData TransactionData, int StatusCode) AddChangeWinTransaction(
        string remoteTransactionId,
        decimal amount,
        decimal previousAmount,
        string privateToken,
        string previousTransactionId)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@RemoteTransactionId", remoteTransactionId, DbType.String, ParameterDirection.Input);
        parameters.Add("@Amount", amount, DbType.Decimal, ParameterDirection.Input);
        parameters.Add("@PreviousAmount", previousAmount, DbType.Decimal, ParameterDirection.Input);
        parameters.Add("@PrivateToken", privateToken, DbType.String, ParameterDirection.Input);
        parameters.Add("@PreviousTransactionId", previousTransactionId, DbType.String, ParameterDirection.Input);
        parameters.Add("@RemoteTransactionIdOut", dbType: DbType.String, size: 100,
            direction: ParameterDirection.Output);
        parameters.Add("@NewCurrentBalance", dbType: DbType.Decimal, direction: ParameterDirection.Output);
        parameters.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);

        try
        {
            dbConnection.Execute(
                "AddChangeWinTransactionProcedure",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            var statusCode = parameters.Get<int>("@StatusCode");

            if (statusCode != 200)
            {
                return (null, statusCode);
            }

            var transactionData = new TransactionResponseData
            {
                TransactionId = parameters.Get<string>("@RemoteTransactionIdOut"),
                CurrentBalance = parameters.Get<decimal>("@NewCurrentBalance")
            };

            return (transactionData, statusCode);
        }
        catch (SqlException e)
        {
            Console.WriteLine(e.Message);
            Console.WriteLine(e.Number);
            throw;
        }
    }
}