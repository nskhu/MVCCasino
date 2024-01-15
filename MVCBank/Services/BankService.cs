namespace MVCBank.Services;

public class BankService : IBankService
{
    public bool ProcessApprove(decimal amount)
    {
        return amount % 2 == 0;
    }
}