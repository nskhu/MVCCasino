using System.Data;
using Dapper;
using MVCCasino.Enums;
using MVCCasino.Models;

namespace MVCCasino.Data.Repository.Dapper;

public class WalletRepositoryDapper(IDbConnection dbConnection) : IWalletRepository
{
    private readonly IDbConnection
        _dbConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));

    public int Create(Wallet wallet)
    {
        var insertQuery = "INSERT INTO Wallets (UserId, CurrentBalance) VALUES (@UserId, @CurrentBalance)";
        return _dbConnection.Execute(insertQuery, wallet);
    }

    public Wallet GetById(int walletId)
    {
        var getByIdQuery = "SELECT * FROM Wallets WHERE WalletId = @WalletId";
        return _dbConnection.QueryFirstOrDefault<Wallet>(getByIdQuery, new { WalletId = walletId });
    }

    public bool Update(Wallet wallet)
    {
        var updateQuery = "UPDATE Wallets SET CurrentBalance = @CurrentBalance WHERE WalletId = @WalletId";
        return _dbConnection.Execute(updateQuery, wallet) > 0;
    }

    public bool Delete(int walletId)
    {
        var deleteQuery = "DELETE FROM Wallets WHERE WalletId = @WalletId";
        return _dbConnection.Execute(deleteQuery, new { WalletId = walletId }) > 0;
    }

    public Wallet GetWalletByUserId(string userId)
    {
        var getByUserIdQuery = "SELECT * FROM Wallets WHERE UserId = @UserId";
        return _dbConnection.QueryFirstOrDefault<Wallet>(getByUserIdQuery, new { UserId = userId });
    }

    public int Withdraw(string userId, decimal amount)
    {
        var parameters = new DynamicParameters();
        parameters.Add("UserId", userId);
        parameters.Add("Amount", amount);
        parameters.Add("TransactionId", dbType: DbType.Int32, direction: ParameterDirection.Output);

        _dbConnection.Execute("WithdrawProcedure", parameters, commandType: CommandType.StoredProcedure);

        var transactionId = parameters.Get<int>("TransactionId");
        return transactionId;
    }

    public void Deposit(TransactionStatusEnum isSuccess, int transactionId, string userId)
    {
        var parameters = new { IsSuccess = isSuccess, TransactionId = transactionId, UserId = userId };
        _dbConnection.Execute("DepositProcedure", parameters, commandType: CommandType.StoredProcedure);
    }

    public void UpdatePendingWithdraw(TransactionStatusEnum approved, int transactionId, string userId)
    {
        var parameters = new { IsSuccess = approved, TransactionId = transactionId, UserId = userId };
        _dbConnection.Execute("UpdatePendingWithdrawProcedure", parameters, commandType: CommandType.StoredProcedure);
    }
}