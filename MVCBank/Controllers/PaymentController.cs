using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MVCBank.Services;
using System.Globalization;

namespace MVCBank.Controllers;

[Route("[controller]")]
public class PaymentController(IPaymentService paymentService) : Controller
{

    [HttpPost("ProcessPayment")]
    public IActionResult ProcessPayment(decimal amount, int transactionId)
    {
        Console.WriteLine("processing payment with amount: " + amount + "and trId: "+ transactionId);
        var isSuccess = paymentService.ProcessPayment(amount);
        Console.WriteLine("for that amount service returned:" + isSuccess);
        
        // HTTP CLIENTIT MIMARTVA UNDA MOXDES KAZINOS API KONTROLERZE
        // SUCCSES
        
        return Ok(isSuccess
            ? new { success = true, message = "payment successful.", transactionId }
            : new { success = false, message = "payment failed.", transactionId });
    }

    [HttpGet("PaymentView")]
    public IActionResult PaymentView(decimal amount, int transactionId, string userId)
    {
        var cultureInfo = CultureInfo.InvariantCulture;
        var formattedAmount = amount.ToString("0.00", cultureInfo);

        ViewData["TransactionId"] = transactionId;
        ViewData["Amount"] = formattedAmount;
        ViewData["UserId"] = userId;

        return View();
    }
}