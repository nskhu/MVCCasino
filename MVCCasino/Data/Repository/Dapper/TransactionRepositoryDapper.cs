using System.Data;
using Dapper;
using MVCCasino.Models;

namespace MVCCasino.Data.Repository.Dapper;

public class TransactionRepositoryDapper(IDbConnection dbConnection) : ITransactionRepository
{
    private readonly IDbConnection _dbConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));

    public int Create(Transaction transaction)
    {
        var insertQuery =
            "INSERT INTO Transactions (UserId, Amount, TransactionType, TransactionStatus, CurrentBalance, TransactionDate) " +
            "OUTPUT INSERTED.TransactionId " +
            "VALUES (@UserId, @Amount, @TransactionType, @TransactionStatus, @CurrentBalance, @TransactionDate)";
    
        return _dbConnection.QuerySingle<int>(insertQuery, transaction);
    }

    public Transaction GetById(int transactionId)
    {
        var getByIdQuery = "SELECT * FROM Transactions WHERE TransactionId = @TransactionId";
        return _dbConnection.QueryFirstOrDefault<Transaction>(getByIdQuery, new { TransactionId = transactionId });
    }

    public bool Update(Transaction transaction)
    {
        var updateQuery = "UPDATE Transactions SET " +
                          "Amount = @Amount, " +
                          "TransactionType = @TransactionType, " +
                          "TransactionStatus = @TransactionStatus, " +
                          "CurrentBalance = @CurrentBalance, " +
                          "TransactionDate = @TransactionDate " +
                          "WHERE TransactionId = @TransactionId";
        return _dbConnection.Execute(updateQuery, transaction) > 0;
    }

    public bool Delete(int transactionId)
    {
        var deleteQuery = "DELETE FROM Transactions WHERE TransactionId = @TransactionId";
        return _dbConnection.Execute(deleteQuery, new { TransactionId = transactionId }) > 0;
    }

    public IEnumerable<Transaction> GetTransactionsByUserId(string userId)
    {
        var getByUserIdQuery = "SELECT * FROM Transactions WHERE UserId = @UserId";
        return _dbConnection.Query<Transaction>(getByUserIdQuery, new { UserId = userId });
    }
}