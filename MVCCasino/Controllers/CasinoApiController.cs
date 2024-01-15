using Microsoft.AspNetCore.Mvc;
using MVCCasino.Models.Requests;
using MVCCasino.Services;

namespace MVCCasino.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CasinoApiController(
        ILogger<CasinoApiController> logger,
        ITransactionService transactionService) : ControllerBase
    {
        [HttpPost("Deposit")]
        public IActionResult Deposit([FromBody] DepositRequest request)
        {
            logger.LogInformation("start deposit procedure");

            var depositResult =
                transactionService.ProcessDeposit(request.IsSuccess, request.TransactionId, request.UserId);

            if (depositResult.Success)
            {
                return Ok(new { success = true, message = "Deposit successful." });
            }

            return BadRequest(new { success = false, message = depositResult.ErrorMessage });
        }
    }
}