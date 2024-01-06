using Microsoft.AspNetCore.Mvc;

namespace MVCBank.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DepositApiController : ControllerBase
{
    [HttpPost("GetRedirectLink")]
    public IActionResult GetRedirectLink()
    {
        var redirectUrl = "https://example.com/payment";
        return Ok(new { success = true, message = "Deposit successful.", redirectUrl });
    }
}