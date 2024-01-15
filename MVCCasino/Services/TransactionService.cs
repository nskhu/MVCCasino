using System.Data.Common;
using MVCCasino.Data.Repository;
using MVCCasino.Enums;
using MVCCasino.Models;
using MVCCasino.Models.Responses;

namespace MVCCasino.Services;

public class TransactionService(
    IWalletRepository walletRepository,
    ITransactionRepository transactionRepository) : ITransactionService
{
    public void CreateWalletByUserId(string userId)
    {
        var wallet = new Wallet { UserId = userId, CurrentBalance = 0m };
        walletRepository.Create(wallet);
    }

    public WithdrawResponse ProcessWithdraw(string userId, decimal amount)
    {
        try
        {
            var transactionId = walletRepository.Withdraw(userId, amount);
            return new WithdrawResponse { Success = true, TransactionId = transactionId };
        }
        catch (DbException dbEx)
        {
            Console.Error.WriteLine($"Withdraw failed due to a database error: {dbEx.Message}");
            return new WithdrawResponse
                { Success = false, ErrorMessage = "Withdraw failed due to a database error. " + dbEx.Message };
        }
    }

    public decimal GetCurrentBalanceByUserId(string userId)
    {
        return walletRepository.GetWalletByUserId(userId).CurrentBalance;
    }

    public IEnumerable<Transaction> GetTransactionHistoryByUserId(string userId)
    {
        return transactionRepository.GetTransactionsByUserId(userId);
    }

    public int CreateNewTransaction(string userId, decimal amount, TransactionTypeEnum transactionType,
        TransactionStatusEnum transactionStatus, decimal currentBalance)
    {
        var transaction = new Transaction
        {
            UserId = userId,
            Amount = amount,
            CurrentBalance = currentBalance,
            TransactionDate = DateTime.Now,
            TransactionStatus = transactionStatus,
            TransactionType = transactionType
        };

        return transactionRepository.Create(transaction);
    }

    public DepositResponse ProcessDeposit(bool isSuccess, int transactionId, string userId)
    {
        try
        {
            walletRepository.Deposit(isSuccess ? TransactionStatusEnum.Approved : TransactionStatusEnum.Rejected,
                transactionId, userId);
            return new DepositResponse { Success = true };
        }
        catch (DbException dbEx)
        {
            Console.Error.WriteLine($"Deposit failed due to a database error: {dbEx.Message}");
            return new DepositResponse
                { Success = false, ErrorMessage = "Deposit failed due to a database error. " + dbEx.Message };
        }
    }

    public WithdrawResponse UpdatePendingWithdraw(bool isSuccess, int transactionId, string userId)
    {
        try
        {
            walletRepository.UpdatePendingWithdraw(isSuccess ? TransactionStatusEnum.Approved : TransactionStatusEnum.Rejected,
                transactionId, userId);
            return new WithdrawResponse { Success = true };
        }
        catch (DbException dbEx)
        {
            Console.Error.WriteLine($"Withdraw failed due to a database error: {dbEx.Message}");
            return new WithdrawResponse
                { Success = false, ErrorMessage = "Withdraw failed due to a database error. " + dbEx.Message };
        }
    }
}