using MVCCasino.Enums;
using MVCCasino.Models;
using MVCCasino.Models.Responses;

namespace MVCCasino.Services;

public interface ITransactionService
{
    void CreateWalletByUserId(string userId);

    DepositResponse ProcessDeposit(string userId, decimal amount);

    WithdrawResponse ProcessWithdraw(string userId, decimal amount);

    decimal GetCurrentBalanceByUserId(string userId);

    IEnumerable<Transaction> GetTransactionHistoryByUserId(string userId);

    int CreateNewTransaction(String userId, decimal amount, TransactionTypeEnum transactionType, TransactionStatusEnum transactionStatus, decimal currentBalance);
}