namespace BsMerchantAPI.Data.Repositories;

public interface IWalletRepository
{
    decimal GetUserBalance(string userId);
}