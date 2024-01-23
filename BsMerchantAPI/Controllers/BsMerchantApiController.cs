using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BsMerchantAPI.Models.Requests;
using BsMerchantAPI.Models.Responses;
using BsMerchantAPI.Models.Responses.ResponseDatas;
using BsMerchantAPI.Services;
using Microsoft.Data.SqlClient;

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
            catch (SqlException sqlException)
            {
                var errorResponse = new MerchantApiResponse<object?>
                {
                    StatusCode = MapSqlExceptionToStatusCode(sqlException.Number),
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
            catch (SqlException sqlException)
            {
                var errorResponse = new MerchantApiResponse<object?>
                {
                    StatusCode = MapSqlExceptionToStatusCode(sqlException.Number),
                    Data = null
                };

                return StatusCode(500, errorResponse);
            }
        }

        /// <summary>
        /// Get player information based on the provided private token.
        /// </summary>
        /// <param name="request">Request object containing the private token.</param>
        /// <returns>Returns player information if successful, or a 500 status code in case of an error.</returns>
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
            catch (SqlException sqlException)
            {
                var errorResponse = new MerchantApiResponse<object?>
                {
                    StatusCode = MapSqlExceptionToStatusCode(sqlException.Number),
                    Data = null
                };

                return StatusCode(500, errorResponse);
            }
        }

        /// <summary>
        /// Places a bet transaction for the user.
        /// </summary>
        /// <param name="request">The bet request data, including the remote transaction ID, amount, and private token.</param>
        /// <returns>Returns a response containing transaction details if successful; otherwise, returns an error response.</returns>
        [HttpPost("Bet")]
        public IActionResult Bet([FromBody] BetRequest request)
        {
            try
            {
                var betResponseData = merchantService.AddBetTransaction(request.RemoteTransactionId, request.Amount,
                    request.PrivateToken
                );

                var response = new MerchantApiResponse<BetResponseData>
                {
                    StatusCode = 200,
                    Data = betResponseData
                };

                return Ok(response);
            }
            catch (SqlException sqlException)
            {
                var errorResponse = new MerchantApiResponse<object?>
                {
                    StatusCode = MapSqlExceptionToStatusCode(sqlException.Number),
                    Data = null
                };

                return StatusCode(500, errorResponse);
            }
        }

        /// <summary>
        /// Processes a win transaction for the user.
        /// </summary>
        /// <param name="request">The win request data, including the remote transaction ID, amount, and private token.</param>
        /// <returns>Returns a response containing transaction details if successful; otherwise, returns an error response.</returns>
        [HttpPost("Win")]
        public IActionResult Win([FromBody] BetRequest request)
        {
            try
            {
                var winResponseData = merchantService.AddWinTransaction(request.RemoteTransactionId, request.Amount,
                    request.PrivateToken
                );

                var response = new MerchantApiResponse<BetResponseData>
                {
                    StatusCode = 200,
                    Data = winResponseData
                };

                return Ok(response);
            }
            catch (SqlException sqlException)
            {
                var errorResponse = new MerchantApiResponse<object?>
                {
                    StatusCode = MapSqlExceptionToStatusCode(sqlException.Number),
                    Data = null
                };

                return StatusCode(500, errorResponse);
            }
        }

        /// <summary>
        /// Cancels a bet transaction.
        /// </summary>
        /// <param name="request">Cancellation request details.</param>
        /// <returns>Response with transaction information.</returns>
        /// <response code="200">Successful cancellation of the bet transaction.</response>
        /// <response code="500">Error during the cancellation process.</response>
        [HttpPost("CancelBet")]
        public IActionResult CancelBet([FromBody] CancelBetRequest request)
        {
            try
            {
                var cancelBetResponseData = merchantService.AddCancelBetTransaction(
                    request.RemoteTransactionId,
                    request.Amount,
                    request.PrivateToken,
                    request.BetTransactionId
                );

                var response = new MerchantApiResponse<TransactionResponseData>
                {
                    StatusCode = 200,
                    Data = cancelBetResponseData
                };

                return Ok(response);
            }
            catch (SqlException sqlException)
            {
                var errorResponse = new MerchantApiResponse<object?>
                {
                    StatusCode = MapSqlExceptionToStatusCode(sqlException.Number),
                    Data = null
                };

                return StatusCode(500, errorResponse);
            }
        }

        /// <summary>
        /// API endpoint for changing a previous win transaction.
        /// </summary>
        /// <remarks>
        /// Updates a previous win transaction by changing the win amount.
        /// </remarks>
        /// <param name="request">The request object containing necessary parameters.</param>
        /// <returns>
        /// <see cref="IActionResult"/> representing the result of the operation.
        /// Returns a JSON response with the new transaction ID and current balance on success (status code 200).
        /// Returns a 500 Internal Server Error response on failure.
        /// </returns>
        [HttpPost("ChangeWin")]
        public IActionResult ChangeWin([FromBody] ChangeWinRequest request)
        {
            try
            {
                var changeWinResponseData = merchantService.AddChangeWinTransaction(
                    request.RemoteTransactionId,
                    request.Amount,
                    request.PreviousAmount,
                    request.PrivateToken,
                    request.previousTransactionId
                );

                var response = new MerchantApiResponse<TransactionResponseData>
                {
                    StatusCode = 200,
                    Data = changeWinResponseData
                };

                return Ok(response);
            }
            catch (SqlException sqlException)
            {
                var errorResponse = new MerchantApiResponse<object?>
                {
                    StatusCode = MapSqlExceptionToStatusCode(sqlException.Number),
                    Data = null
                };

                return StatusCode(500, errorResponse);
            }
        }

        private int MapSqlExceptionToStatusCode(int sqlExceptionNumber)
        {
            return sqlExceptionNumber switch
            {
                50000 => 404, // Invalid Token, Public token does not exist
                50001 => 401, // Inactive Token, Public token is already expired
                50002 => 201, // Already Processed Transaction (should be used in win and cancelbet methods)
                50003 => 402, // Insufficient Balance
                50004 => 408, // Duplicated TransactionId (should be used in bet, deposit and withdraw methods)
                50005 => 200, // Transaction not found in merchant's system
                50006 => 407, // Invalid Amount
                50007 => 411, // Invalid Request
                _ => 500
            };
        }
    }
}