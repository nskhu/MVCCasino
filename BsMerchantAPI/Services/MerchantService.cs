using BsMerchantAPI.Data.Repositories;
using BsMerchantAPI.Models.Responses.ResponseDatas;

namespace BsMerchantAPI.Services
{
    public class MerchantService(
        IWalletRepository walletRepository) : IMerchantService
    {

        public decimal GetBalance(string privateToken)
        {
            return walletRepository.GetBalance(privateToken);
        }

        public PlayerInfoData? GetPlayerInfo(string privateToken)
        {
            return walletRepository.GetPlayerInfo(privateToken);
        }
    }
}
