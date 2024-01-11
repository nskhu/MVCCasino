using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MVCBank.Services;

namespace MVCBank.Controllers;

public class PaymentController(IPaymentService paymentService) : Controller
{
    public IActionResult ProcessPayment(decimal amount, int transactionId)
    {
        Console.WriteLine("processing payment with amount: " + amount + "and trId: "+ transactionId);
        var isSuccess = paymentService.ProcessPayment(amount);
        Console.WriteLine("for that amount service returned:" + isSuccess);
        return Ok(isSuccess
            ? new { success = true, message = "payment successful.", transactionId }
            : new { success = false, message = "payment failed.", transactionId });
    }

    [HttpGet("PaymentView")]
    public IActionResult PaymentView(decimal amount, int transactionId)
    {
        Console.WriteLine("in controller payment view action: " + amount + " id " + transactionId);
        ViewData["TransactionId"] = transactionId;
        ViewData["Amount"] = amount;
        return View();
    }
}