namespace MVCBank.Models.Requests
{
    public class WithdrawRequest
    {
        public string UserId { get; set; }
        public int TransactionId { get; set; }
        public decimal Amount { get; set; }
    }
}
