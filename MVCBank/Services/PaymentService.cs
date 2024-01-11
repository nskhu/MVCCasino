namespace MVCBank.Services;

public class PaymentService : IPaymentService
{
    public bool ProcessPayment(decimal amount)
    {
        return amount % 2 == 0;
    }
}