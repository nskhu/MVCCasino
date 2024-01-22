using BsMerchantAPI.Models.Responses.ResponseDatas;

namespace BsMerchantAPI.Services
{
    public interface IMerchantService
    {
        decimal GetBalance(string privateToken);

        PlayerInfoData? GetPlayerInfo(string requestPrivateToken);

        BetResponseData AddBetTransaction(string remoteTransactionId, decimal amount, string privateToken);

        BetResponseData? AddWinTransaction(string remoteTransactionId, decimal amount, string privateToken);

        TransactionResponseData? AddCancelBetTransaction(string remoteTransactionId, decimal amount,
            string privateToken, string betTransactionId);

        TransactionResponseData? AddChangeWinTransaction(string remoteTransactionId, decimal amount,
            decimal previousAmount, string privateToken, string previousTransactionId);
    }
}