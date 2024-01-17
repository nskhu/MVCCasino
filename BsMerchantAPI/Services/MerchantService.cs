using BsMerchantAPI.Data.Repositories;

namespace BsMerchantAPI.Services
{
    public class MerchantService(
        IWalletRepository walletRepository) : IMerchantService
    {
        public decimal GetUserBalance(string userId)
        {
            return walletRepository.GetUserBalance(userId);
        }
    }
}
