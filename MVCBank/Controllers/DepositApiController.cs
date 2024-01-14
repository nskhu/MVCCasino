using Microsoft.AspNetCore.Mvc;

namespace MVCBank.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DepositApiController(ILogger<DepositApiController> logger) : ControllerBase
{
    [HttpPost("GetRedirectLink")]
    public IActionResult GetRedirectLink([FromBody] BankRequestModel request)
    {
        var bankPaymentViewUrl = "http://localhost:7195/Payment/PaymentView";
        logger.LogInformation("test");

        return Ok(new { Success = true, Message = "URL generated successfully.", RedirectUrl = bankPaymentViewUrl });
    }

    public class BankRequestModel
    {
        public string UserId { get; set; }
        public int TransactionId { get; set; }
        public decimal Amount { get; set; }
    }
}