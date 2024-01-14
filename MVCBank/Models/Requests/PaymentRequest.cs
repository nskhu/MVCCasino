namespace MVCBank.Models.Requests
{
    public class PaymentRequest
    {
        public string UserId { get; set; }
        public int TransactionId { get; set; }
        public decimal Amount { get; set; }
    }
}
