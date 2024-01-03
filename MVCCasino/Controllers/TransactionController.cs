using Microsoft.AspNetCore.Mvc;
using MVCCasino.Services;

namespace MVCCasino.Controllers
{
    [Route("api/[controller]")]
    public class TransactionController(ITransactionService transactionService) : Controller
    {
        [HttpPost("deposit")]
        public IActionResult Deposit(decimal amount)
        {
            if (User.Identity is not { IsAuthenticated: true })
            {
                return Unauthorized(new { message = "User is not authenticated." });
            }

            // Validate the amount (you can add more complex validations as needed)
            // TODO check for amount balance numeric
            if (amount <= 0)
            {
                return BadRequest(new { message = "Invalid deposit amount." });
            }

            var userId = User.Identity.Name;

            var depositResult = transactionService.ProcessDeposit(userId, amount);

            if (depositResult.Success)
            {
                return Ok(new { success = true, message = "Deposit successful." });
            }
            else
            {
                return BadRequest(new { success = false, message = depositResult.ErrorMessage });
            }
        }
    }
}