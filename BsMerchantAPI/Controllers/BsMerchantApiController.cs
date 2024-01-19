using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using BsMerchantAPI.Models.Requests;
using BsMerchantAPI.Models.Responses;
using BsMerchantAPI.Models.Responses.ResponseDatas;
using BsMerchantAPI.Services;

namespace BsMerchantAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BsMerchantApiController(
        ILogger<BsMerchantApiController> logger,
        IAuthService authService,
        IMerchantService merchantService) : ControllerBase
    {
        [HttpPost("GetCurrentBalance")]
        public IActionResult GetCurrentBalance()
        {
            const string userId = "115da0de-4261-4223-84cc-546dea3f2024";
            var currentBalance = merchantService.GetUserBalance(userId);

            return Ok(new { balance = currentBalance });
        }

        [HttpPost("Auth")]
        public IActionResult Auth([FromBody] PublicTokenRequest request)
        {
            try
            {
                var privateToken = authService.GeneratePrivateToken(request.PublicToken);

                var response = new MerchantApiResponse<AuthResponseData>
                {
                    StatusCode = 200,
                    Data = new AuthResponseData()
                    {
                        PrivateToken = privateToken
                    }
                };

                return Ok(response);
            }
            catch
            {
                var errorResponse = new MerchantApiResponse<object?>
                {
                    StatusCode = 500,
                    Data = null
                };

                return StatusCode(500, errorResponse);
            }
        }
    }
}