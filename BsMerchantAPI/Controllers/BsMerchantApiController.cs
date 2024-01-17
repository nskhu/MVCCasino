using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using BsMerchantAPI.Services;

namespace BsMerchantAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BsMerchantApiController(
        ILogger<BsMerchantApiController> logger,
        IMerchantService merchantService) : ControllerBase
    {

        [HttpPost("GetCurrentBalance")]
        public IActionResult GetCurrentBalance()
        {
            const string userId = "115da0de-4261-4223-84cc-546dea3f2024";
            var currentBalance = merchantService.GetUserBalance(userId);

            return Ok(new { balance = currentBalance });
        }
    }
}
