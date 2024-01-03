using MVCCasino.Models.Responses;

namespace MVCCasino.Services;

public interface ITransactionService
{
    void CreateWalletByUserId(string userId);

    DepositResponse ProcessDeposit(string userId, decimal amount);

    WithdrawResponse ProcessWithdraw(string userId, decimal amount);
}