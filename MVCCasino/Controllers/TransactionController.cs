﻿using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using MVCCasino.Services;

namespace MVCCasino.Controllers;

[Route("api/[controller]")]
public class TransactionController(ITransactionService transactionService) : Controller
{
    [HttpPost("deposit")]
    public IActionResult Deposit(decimal amount)
    {
        if (User.Identity is not { IsAuthenticated: true })
            return Unauthorized(new { message = "User is not authenticated." });

        if (amount <= 0) return BadRequest(new { message = "Invalid deposit amount." });

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var depositResult = transactionService.ProcessDeposit(userId, amount);

        if (depositResult.Success)
            return Ok(new { success = true, message = "Deposit successful." });
        return BadRequest(new { success = false, message = depositResult.ErrorMessage });
    }

    [HttpPost("withdraw")]
    public IActionResult Withdraw(decimal amount)
    {
        if (User.Identity is not { IsAuthenticated: true })
            return Unauthorized(new { message = "User is not authenticated." });

        // TODO check for amount < balance
        if (amount <= 0) return BadRequest(new { message = "Invalid deposit amount." });

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var withdrawResult = transactionService.ProcessWithdraw(userId, amount);

        if (withdrawResult.Success)
            return Ok(new { success = true, message = "Withdraw successful." });
        return BadRequest(new { success = false, message = withdrawResult.ErrorMessage });
    }
}