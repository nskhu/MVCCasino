using MVCCasino.Models;

namespace MVCCasino.Data.Repository;

public interface IWalletRepository : ICrudRepository<Wallet>
{
    Wallet GetWalletByUserId(string userId);
}