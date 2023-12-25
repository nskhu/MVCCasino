using MVCCasino.Enums;

namespace MVCCasino.Models;

public class Transaction
{
    public int TransactionId { get; set; }
    public string UserId { get; set; }
    public decimal Amount { get; set; }
    public TransactionTypeEnum TransactionType { get; set; }
    public TransactionStatusEnum TransactionStatus { get; set; }
    public decimal CurrentBalance { get; set; }
    public DateTime TransactionDate { get; set; }
}