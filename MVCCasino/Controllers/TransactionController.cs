using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using MVCCasino.Services;

namespace MVCCasino.Controllers;

[Route("api/[controller]")]
public class TransactionController(ITransactionService transactionService) : Controller
{
    [HttpGet("DepositView")]
    public IActionResult DepositView()
    {
        return View();
    }

    [HttpGet("WithdrawView")]
    public IActionResult WithdrawView()
    {
        return View();
    }

    [HttpPost("deposit")]
    public IActionResult Deposit(decimal amount)
    {
        Console.WriteLine("start transaction controller deposit action");

        if (User.Identity is not { IsAuthenticated: true })
            return Unauthorized(new { message = "User is not authenticated." });

        if (amount <= 0) return BadRequest(new { message = "Invalid deposit amount." });

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var depositResult = transactionService.ProcessDeposit(userId, amount);

        if (depositResult.Success)
        {
            Console.WriteLine("in transaction controller deposit result is success, return url to js");
            var redirectUrl = Url.Action("Index", "Home");
            return Ok(new { success = true, message = "Deposit successful.", redirectUrl });
        }

        return BadRequest(new { success = false, message = depositResult.ErrorMessage });
    }

    [HttpPost("withdraw")]
    public IActionResult Withdraw(decimal amount)
    {
        if (User.Identity is not { IsAuthenticated: true })
            return Unauthorized(new { message = "User is not authenticated." });

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var currentBalance = transactionService.GetCurrentBalanceByUserId(userId);
        if (amount > currentBalance)
            return BadRequest(new { message = "Withdrawal amount exceeds available balance." });

        if (amount <= 0)
            return BadRequest(new { message = "Invalid withdraw amount." });

        var withdrawResult = transactionService.ProcessWithdraw(userId, amount);

        if (withdrawResult.Success)
        {
            var redirectUrl = Url.Action("Index", "Home");
            return Ok(new { success = true, message = "Withdraw successful.", redirectUrl });
        }

        return BadRequest(new { success = false, message = withdrawResult.ErrorMessage });
    }

    [HttpGet("balance")]
    public IActionResult GetCurrentBalance()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var currentBalance = transactionService.GetCurrentBalanceByUserId(userId);
        return Json(new { currentBalance });
    }
}