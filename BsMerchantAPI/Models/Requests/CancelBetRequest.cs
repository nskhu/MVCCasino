namespace BsMerchantAPI.Models.Requests;

public class CancelBetRequest
{
    public string PrivateToken { get; set; }
    public decimal Amount { get; set; }
    public string RemoteTransactionId { get; set; }
    public string BetTransactionId { get; set; }
}