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

        public BetResponseData AddBetTransaction(string remoteTransactionId, decimal amount, string privateToken)
        {
            return walletRepository.AddBetTransaction(remoteTransactionId, amount, privateToken);
        }

        public BetResponseData? AddWinTransaction(string remoteTransactionId, decimal amount, string privateToken)
        {
            return walletRepository.AddWinTransaction(remoteTransactionId, amount, privateToken);
        }

        public TransactionResponseData? AddCancelBetTransaction(string remoteTransactionId, decimal amount, string privateToken,
            string betTransactionId)
        {
            return walletRepository.AddCancelBetTransaction(remoteTransactionId, amount, privateToken, betTransactionId);

        }
    }
}