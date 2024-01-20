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
        /// <summary>
        /// Authenticate the user and generate a private token.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// {
        ///   "PublicToken": "your_public_token_here"
        /// }
        /// </remarks>
        /// <param name="request">Public token request.</param>
        /// <returns>Returns the private token if successful.</returns>
        /// <response code="200">Returns the private token if authentication is successful.</response>
        /// <response code="500">Returns an error response if an unexpected error occurs.</response>
        [HttpPost("Auth")]
        [ProducesResponseType(typeof(MerchantApiResponse<AuthResponseData>), 200)]
        [ProducesResponseType(typeof(MerchantApiResponse<object>), 500)]
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

        /// <summary>
        /// Get the balance for a user using a private token.
        /// </summary>
        /// <remarks>
        /// Retrieves the user's balance based on the provided private token.
        /// </remarks>
        /// <param name="request">The request object containing the private token.</param>
        /// <returns>Returns the user's balance.</returns>
        /// <response code="200">Successful operation. Returns the user's balance.</response>
        /// <response code="500">Internal server error. Occurs if there is an issue retrieving the balance.</response>
        [HttpPost("GetBalance")]
        [ProducesResponseType(typeof(MerchantApiResponse<BalanceResponseData>), 200)]
        [ProducesResponseType(typeof(MerchantApiResponse<object?>), 500)]
        public IActionResult GetBalance([FromBody] PrivateTokenRequest request)
        {
            try
            {
                var balance = merchantService.GetBalance(request.PrivateToken);

                var response = new MerchantApiResponse<BalanceResponseData>
                {
                    StatusCode = 200,
                    Data = new BalanceResponseData()
                    {
                        Balance = balance
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

        [HttpPost("GetPlayerInfo")]
        public IActionResult GetPlayerInfo([FromBody] PrivateTokenRequest request)
        {
            try
            {
                var playerInfo = merchantService.GetPlayerInfo(request.PrivateToken);

                var response = new MerchantApiResponse<PlayerInfoData>
                {
                    StatusCode = 200,
                    Data = playerInfo
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