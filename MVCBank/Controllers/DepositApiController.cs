using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MVCBank.Models.Requests;
using MVCBank.Settings;
using System.Globalization;

namespace MVCBank.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DepositApiController(
    ILogger<DepositApiController> logger,
    IOptions<BankApiSettings> bankApiSettings) : ControllerBase
{
    [HttpPost("GetRedirectLink")]
    public IActionResult GetRedirectLink([FromBody] PaymentRequest request)
    {
        var bankPaymentViewUrl = bankApiSettings.Value.PaymentUrl;
        var redirectUrl =
            $"{bankPaymentViewUrl}?UserId={Uri.EscapeDataString(request.UserId)}&TransactionId={request.TransactionId}&Amount={request.Amount.ToString("0.00", CultureInfo.InvariantCulture)}";


        logger.LogInformation("redirectUrl: " + redirectUrl);

        return Ok(new { Success = true, Message = "URL generated successfully.", RedirectUrl = redirectUrl });
    }
}