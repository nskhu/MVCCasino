using MVCCasino.Models;

namespace MVCCasino.Data.Repository;

public interface IWalletRepository : ICrudRepository<Wallet>
{
    Wallet GetWalletByUserId(string userId);

    void Deposit(string userId, decimal amount);

    void Withdraw(string userId, decimal amount);
}