using BsMerchantAPI.Models.Responses.ResponseDatas;

namespace BsMerchantAPI.Data.Repositories;

public interface IWalletRepository
{
    decimal GetBalance(string privateToken);
    PlayerInfoData GetPlayerInfo(string privateToken);
}