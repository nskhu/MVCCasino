using MVCCasino.Enums;
using MVCCasino.Models;

namespace MVCCasino.Data.Repository;

public interface IWalletRepository : ICrudRepository<Wallet>
{
    Wallet GetWalletByUserId(string userId);
    void Withdraw(string userId, decimal amount);
    void Deposit(TransactionStatusEnum isSuccess, int transactionId, string userId);
}