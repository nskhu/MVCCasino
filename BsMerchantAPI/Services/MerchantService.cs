using BsMerchantAPI.Data.Repositories;

namespace BsMerchantAPI.Services
{
    public class MerchantService(
        IWalletRepository walletRepository) : IMerchantService
    {

        public decimal GetBalance(string privateToken)
        {
            return walletRepository.GetBalance(privateToken);
        }
    }
}
