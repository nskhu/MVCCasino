using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MVCCasino.Enums;
using MVCCasino.Models.Responses;
using MVCCasino.Services;
using MVCCasino.Settings;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text;

namespace MVCCasino.Controllers;

[Route("[controller]")]
public class TransactionController(
    ITransactionService transactionService,
    ILogger<TransactionController> logger,
    IOptions<BankApiSettings> bankApiSettings,
    HttpClient httpClient) : Controller
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

    [HttpPost("CreateDepositTransaction")]
    public IActionResult CreateDepositTransaction(decimal amount)
    {
        if (User.Identity is not { IsAuthenticated: true })
            return Unauthorized(new { message = "User is not authenticated." });

        if (amount <= 0)
            return BadRequest(new { message = "Invalid deposit amount." });

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var transactionId = transactionService.CreateNewTransaction(userId, amount, TransactionTypeEnum.Deposit,
            TransactionStatusEnum.Pending, transactionService.GetCurrentBalanceByUserId(userId));
        var redirectUrl = GetPaymentUrlFromBank(userId, transactionId, amount);

        logger.LogInformation("transaction registered as pending. returning bank payment url to redirect: " +
                              redirectUrl);

        return Ok(new { success = true, message = "transaction saved successfully.", redirectUrl });
    }
    
    [HttpPost("Withdraw")]
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

    [HttpGet("GetCurrentBalance")]
    public IActionResult GetCurrentBalance()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var currentBalance = transactionService.GetCurrentBalanceByUserId(userId);

        return Json(new { currentBalance });
    }

    [HttpGet("Transactions")]
    public IActionResult GetTransactionHistory()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var transactions = transactionService.GetTransactionHistoryByUserId(userId);

        return View("TransactionHistory", transactions);
    }

    private string GetPaymentUrlFromBank(string userId, int transactionId, decimal amount)
    {
        try
        {
            var apiUrl = bankApiSettings.Value.ApiUrl;
            var redirectUrl = string.Empty;
            var content = new StringContent(JsonConvert.SerializeObject(new
            {
                UserId = userId,
                TransactionId = transactionId,
                Amount = amount
            }), Encoding.UTF8, "application/json");
            var response = httpClient.PostAsync(apiUrl, content).Result;
            response.EnsureSuccessStatusCode();
            var responseJson = response.Content.ReadAsStringAsync().Result;
            var bankApiResponse = JsonConvert.DeserializeObject<BankApiResponse>(responseJson);

            if (bankApiResponse is { Success: true })
            {
                redirectUrl = bankApiResponse.RedirectUrl;
            }

            return redirectUrl;
        }
        catch (HttpRequestException httpEx)
        {
            logger.LogError($"HTTP Error: {httpEx.Message}");
            return null;
        }
        catch (OperationCanceledException canceledEx)
        {
            logger.LogError($"Task Canceled: {canceledEx.Message}");
            return null;
        }
    }
}