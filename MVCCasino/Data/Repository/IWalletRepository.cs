using MVCCasino.Enums;
using MVCCasino.Models;

namespace MVCCasino.Data.Repository;

public interface IWalletRepository : ICrudRepository<Wallet>
{
    Wallet GetWalletByUserId(string userId);
    int Withdraw(string userId, decimal amount);
    void Deposit(TransactionStatusEnum isSuccess, int transactionId, string userId);
    void UpdatePendingWithdraw(TransactionStatusEnum approved, int transactionId, string userId);
}