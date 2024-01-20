using BsMerchantAPI.Models.Responses.ResponseDatas;

namespace BsMerchantAPI.Services
{
    public interface IMerchantService
    {
        decimal GetBalance(string privateToken);
        PlayerInfoData? GetPlayerInfo(string requestPrivateToken);
    }
}
