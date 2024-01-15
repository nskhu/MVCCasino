using Microsoft.AspNetCore.Mvc;
using MVCBank.Services;
using System.Globalization;
using System.Text;
using Newtonsoft.Json;
using MVCBank.Models.Responses;
using MVCBank.Models.Requests;
using Microsoft.Extensions.Options;
using MVCBank.Settings;

namespace MVCBank.Controllers;

[Route("[controller]")]
public class PaymentController(
    IBankService bankService,
    HttpClient httpClient,
    IOptions<BankApiSettings> bankApiSettings,
    ILogger<PaymentController> logger) : Controller
{
    [HttpPost("ProcessPayment")]
    public IActionResult ProcessPayment([FromBody] PaymentRequest request)
    {
        var amount = request.Amount;
        var transactionId = request.TransactionId;
        var userId = request.UserId;
        var redirectUrl = bankApiSettings.Value.CasinoHomeUrl;
        var isSuccess = bankService.ProcessApprove(amount);
        var casinoResponse = SendBankPaymentVerificationResponse(isSuccess, transactionId, userId);

        return Ok(isSuccess
            ? new { success = true, message = "payment successful.", transactionId, redirectUrl }
            : new { success = false, message = "payment failed.", transactionId, redirectUrl });
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
            var apiUrl = bankApiSettings.Value.CasinoDepositUrl;
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