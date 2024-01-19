namespace BsMerchantAPI.Data.Repositories;

public interface IWalletRepository
{
    decimal GetBalance(string privateToken);
}