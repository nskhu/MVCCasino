namespace BsMerchantAPI.Services
{
    public interface IMerchantService
    {
        decimal GetBalance(string privateToken);
    }
}
