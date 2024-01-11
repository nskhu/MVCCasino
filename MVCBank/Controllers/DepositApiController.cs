using Microsoft.AspNetCore.Mvc;

namespace MVCBank.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DepositApiController : ControllerBase
{
    [HttpPost("GetRedirectLink")]
    public IActionResult GetRedirectLink()
    {
        var bankPaymentViewUrl = GenerateBankPaymentViewUrl();

        return Ok(new { success = true, message = "URL generated successfully.", redirectUrl = bankPaymentViewUrl });
    }

    private string GenerateBankPaymentViewUrl()
    {
        return "http://localhost:5264/PaymentView";
    }
}