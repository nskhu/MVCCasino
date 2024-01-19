namespace BsMerchantAPI.Services
{
    public interface IMerchantService
    {
        decimal GetUserBalance(string userId);
        decimal GetBalance(string privateToken);
    }
}
