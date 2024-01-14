namespace MVCCasino.Models.Responses
{
    public class BankApiResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string RedirectUrl { get; set; }
    }
}