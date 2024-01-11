namespace MVCBank.Services;

public interface IPaymentService
{
    bool ProcessPayment(decimal amount);
}