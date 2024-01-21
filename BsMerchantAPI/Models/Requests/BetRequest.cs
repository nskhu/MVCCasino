namespace BsMerchantAPI.Models.Requests;

public class BetRequest
{
    public string PrivateToken { get; set; }
    public decimal Amount { get; set; }
    public string RemoteTransactionId { get; set; }
}