using Microsoft.AspNetCore.Mvc;

namespace MVCBank.Controllers;

public class PaymentController : Controller
{
    public IActionResult Payment(int transactionId, decimal amount)
    {
        // Your logic for processing payment, handling transactionId, and amount

        // For simplicity, let's assume you generate a unique payment URL for each transaction
        var paymentUrl = Url.Action("PaymentView", "Payment", new { transactionId, amount }, Request.Scheme);

        return Ok(new { success = true, message = "Payment successful.", paymentUrl });
    }

    public IActionResult PaymentView(int transactionId, decimal amount)
    {
        // Your logic for rendering the Payment view
        ViewData["TransactionId"] = transactionId;
        ViewData["Amount"] = amount;

        return View();
    }
}