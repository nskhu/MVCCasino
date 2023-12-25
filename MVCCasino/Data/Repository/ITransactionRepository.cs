using MVCCasino.Models;

namespace MVCCasino.Data.Repository;

public interface ITransactionRepository : ICrudRepository<Transaction>
{
    IEnumerable<Transaction> GetTransactionsByUserId(string userId);
}