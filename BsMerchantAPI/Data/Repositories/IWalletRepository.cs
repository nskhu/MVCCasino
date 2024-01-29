using BsMerchantAPI.Models.Responses.ResponseDatas;

namespace BsMerchantAPI.Data.Repositories;

public interface IWalletRepository
{
    (decimal Balance, int StatusCode) GetBalance(string privateToken);

    (PlayerInfoData PlayerInfo, int StatusCode) GetPlayerInfo(string privateToken);

    (BetResponseData BetResponse, int StatusCode) AddBetTransaction(string remoteTransactionId, decimal amount,
        string privateToken);

    (BetResponseData WinResponse, int StatusCode) AddWinTransaction(string remoteTransactionId, decimal amount,
        string privateToken);

    (TransactionResponseData TransactionData, int StatusCode) AddCancelBetTransaction(string remoteTransactionId,
        decimal amount, string privateToken,
        string betTransactionId);

    (TransactionResponseData TransactionData, int StatusCode) AddChangeWinTransaction(string remoteTransactionId,
        decimal amount, decimal previousAmount,
        string privateToken, string previousTransactionId);
}