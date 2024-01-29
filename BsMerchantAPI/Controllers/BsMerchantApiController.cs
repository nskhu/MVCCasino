using BsMerchantAPI.Data.Repositories;
using BsMerchantAPI.Models.Requests;
using BsMerchantAPI.Models.Responses;
using BsMerchantAPI.Models.Responses.ResponseDatas;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace BsMerchantAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BsMerchantApiController(
    ILogger<BsMerchantApiController> logger,
    IAuthRepository authRepository,
    IWalletRepository walletRepository) : ControllerBase
{
    /// <summary>
    ///     Authenticate the user and generate a private token.
    /// </summary>
    /// <remarks>
    ///     Sample request:
    ///     {
    ///     "PublicToken": "your_public_token_here"
    ///     }
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
            var tokenWithStatus = authRepository.GeneratePrivateToken(request.PublicToken);

            var response = new MerchantApiResponse<AuthResponseData>
            {
                StatusCode = tokenWithStatus.StatusCode,
                Data = new AuthResponseData
                {
                    PrivateToken = tokenWithStatus.PrivateToken
                }
            };

            return Ok(response);
        }
        catch (SqlException sqlException)
        {
            var errorResponse = new MerchantApiResponse<object?>
            {
                StatusCode = sqlException.Number,
                Data = null
            };

            return StatusCode(500, errorResponse);
        }
    }

    /// <summary>
    ///     Get the balance for a user using a private token.
    /// </summary>
    /// <remarks>
    ///     Retrieves the user's balance based on the provided private token.
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
            var balanceWithStatus = walletRepository.GetBalance(request.PrivateToken);

            var response = new MerchantApiResponse<BalanceResponseData>
            {
                StatusCode = balanceWithStatus.StatusCode,
                Data = new BalanceResponseData
                {
                    Balance = balanceWithStatus.Balance
                }
            };

            return Ok(response);
        }
        catch (SqlException sqlException)
        {
            var errorResponse = new MerchantApiResponse<object?>
            {
                StatusCode = sqlException.Number,
                Data = null
            };

            return StatusCode(500, errorResponse);
        }
    }

    /// <summary>
    ///     Get player information based on the provided private token.
    /// </summary>
    /// <param name="request">Request object containing the private token.</param>
    /// <returns>Returns player information if successful, or a 500 status code in case of an error.</returns>
    [HttpPost("GetPlayerInfo")]
    public IActionResult GetPlayerInfo([FromBody] PrivateTokenRequest request)
    {
        try
        {
            var playerInfoWithStatus = walletRepository.GetPlayerInfo(request.PrivateToken);

            var response = new MerchantApiResponse<PlayerInfoData>
            {
                StatusCode = playerInfoWithStatus.StatusCode,
                Data = playerInfoWithStatus.PlayerInfo
            };

            return Ok(response);
        }
        catch (SqlException sqlException)
        {
            var errorResponse = new MerchantApiResponse<object?>
            {
                StatusCode = sqlException.Number,
                Data = null
            };

            return StatusCode(500, errorResponse);
        }
    }

    /// <summary>
    ///     Places a bet transaction for the user.
    /// </summary>
    /// <param name="request">The bet request data, including the remote transaction ID, amount, and private token.</param>
    /// <returns>Returns a response containing transaction details if successful; otherwise, returns an error response.</returns>
    [HttpPost("Bet")]
    public IActionResult Bet([FromBody] BetRequest request)
    {
        try
        {
            var betResponseDataWithStatus = walletRepository.AddBetTransaction(request.RemoteTransactionId,
                request.Amount,
                request.PrivateToken
            );

            var response = new MerchantApiResponse<BetResponseData>
            {
                StatusCode = betResponseDataWithStatus.StatusCode,
                Data = betResponseDataWithStatus.BetResponse
            };

            return Ok(response);
        }
        catch (SqlException sqlException)
        {
            var errorResponse = new MerchantApiResponse<object?>
            {
                StatusCode = sqlException.Number,
                Data = null
            };

            return StatusCode(500, errorResponse);
        }
    }

    /// <summary>
    ///     Processes a win transaction for the user.
    /// </summary>
    /// <param name="request">The win request data, including the remote transaction ID, amount, and private token.</param>
    /// <returns>Returns a response containing transaction details if successful; otherwise, returns an error response.</returns>
    [HttpPost("Win")]
    public IActionResult Win([FromBody] BetRequest request)
    {
        try
        {
            var winResponseData = walletRepository.AddWinTransaction(request.RemoteTransactionId, request.Amount,
                request.PrivateToken
            );

            var response = new MerchantApiResponse<BetResponseData>
            {
                StatusCode = winResponseData.StatusCode,
                Data = winResponseData.WinResponse
            };

            return Ok(response);
        }
        catch (SqlException sqlException)
        {
            var errorResponse = new MerchantApiResponse<object?>
            {
                StatusCode = sqlException.Number,
                Data = null
            };

            return StatusCode(500, errorResponse);
        }
    }

    /// <summary>
    ///     Cancels a bet transaction.
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
            var cancelBetResponseData = walletRepository.AddCancelBetTransaction(
                request.RemoteTransactionId,
                request.Amount,
                request.PrivateToken,
                request.BetTransactionId
            );

            var response = new MerchantApiResponse<TransactionResponseData>
            {
                StatusCode = cancelBetResponseData.StatusCode,
                Data = cancelBetResponseData.TransactionData
            };

            return Ok(response);
        }
        catch (SqlException sqlException)
        {
            var errorResponse = new MerchantApiResponse<object?>
            {
                StatusCode = sqlException.Number,
                Data = null
            };

            return StatusCode(500, errorResponse);
        }
    }

    /// <summary>
    ///     API endpoint for changing a previous win transaction.
    /// </summary>
    /// <remarks>
    ///     Updates a previous win transaction by changing the win amount.
    /// </remarks>
    /// <param name="request">The request object containing necessary parameters.</param>
    /// <returns>
    ///     <see cref="IActionResult" /> representing the result of the operation.
    ///     Returns a JSON response with the new transaction ID and current balance on success (status code 200).
    ///     Returns a 500 Internal Server Error response on failure.
    /// </returns>
    [HttpPost("ChangeWin")]
    public IActionResult ChangeWin([FromBody] ChangeWinRequest request)
    {
        try
        {
            var changeWinResponseData = walletRepository.AddChangeWinTransaction(
                request.RemoteTransactionId,
                request.Amount,
                request.PreviousAmount,
                request.PrivateToken,
                request.previousTransactionId
            );

            var response = new MerchantApiResponse<TransactionResponseData>
            {
                StatusCode = changeWinResponseData.StatusCode,
                Data = changeWinResponseData.TransactionData
            };

            return Ok(response);
        }
        catch (SqlException sqlException)
        {
            var errorResponse = new MerchantApiResponse<object?>
            {
                StatusCode = sqlException.Number,
                Data = null
            };

            return StatusCode(500, errorResponse);
        }
    }
}