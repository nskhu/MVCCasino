namespace MVCCasino.Models.Requests
{
    public class DepositRequest
    {
        public bool IsSuccess { get; set; }
        public int TransactionId { get; set; }
        public string UserId { get; set; }
    }
}
