namespace MVCBank.Services;

public interface IBankService
{
    bool ProcessApprove(decimal amount);
}