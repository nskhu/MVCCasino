namespace BsMerchantAPI.Models.Requests;

public class ChangeWinRequest
{
    public string PrivateToken { get; set; }
    public decimal Amount { get; set; }
    public decimal PreviousAmount { get; set; }
    public string RemoteTransactionId { get; set; }
    public string previousTransactionId { get; set; }
}