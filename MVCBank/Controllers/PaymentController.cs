using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using MVCBank.Services;
using System.Globalization;
using System.Text;
using Newtonsoft.Json;
using MVCBank.Models.Responses;
using MVCBank.Models.Requests;

namespace MVCBank.Controllers;

[Route("[controller]")]
public class PaymentController(
    IBankService bankService,
    HttpClient httpClient,
    ILogger<PaymentController> logger) : Controller
{
    [HttpPost("ProcessApprove")]
    public IActionResult ProcessPayment([FromBody] PaymentRequest request)
    {
        var amount = request.Amount;
        var transactionId = request.TransactionId;
        var userId = request.UserId;

        var isSuccess = bankService.ProcessApprove(amount);
        var casinoResponse = SendBankPaymentVerificationResponse(isSuccess, transactionId, userId);

        return Ok(isSuccess
            ? new { success = true, message = "payment successful.", transactionId }
            : new { success = false, message = "payment failed.", transactionId });
    }

    [HttpGet("PaymentView")]
    public IActionResult PaymentView(decimal amount, int transactionId, string userId)
    {
        var formattedAmount = amount.ToString("0.00", CultureInfo.InvariantCulture);
        ViewData["TransactionId"] = transactionId;
        ViewData["Amount"] = formattedAmount;
        ViewData["UserId"] = userId;

        return View();
    }

    private bool SendBankPaymentVerificationResponse(bool isSuccess, int transactionId, string userId)
    {
        try
        {
            var apiUrl = "https://localhost:7190/api/CasinoApi/deposit";
            var content = new StringContent(JsonConvert.SerializeObject(new
            {
                IsSuccess = isSuccess,
                TransactionId = transactionId,
                UserId = userId
            }), Encoding.UTF8, "application/json");
            var response = httpClient.PostAsync(apiUrl, content).Result;
            response.EnsureSuccessStatusCode();
            var responseJson = response.Content.ReadAsStringAsync().Result;
            var casinoDepositResponse = JsonConvert.DeserializeObject<CasinoDepositResponse>(responseJson);

            if (casinoDepositResponse is { Success: true })
            {
            }

            return casinoDepositResponse.Success;
        }
        catch (HttpRequestException httpEx)
        {
            logger.LogError($"HTTP Error: {httpEx.Message}");
            return false;
        }
        catch (OperationCanceledException canceledEx)
        {
            logger.LogError($"Task Canceled: {canceledEx.Message}");
            return false;
        }
    }
}