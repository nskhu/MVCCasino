using System.Data.Common;
using MVCCasino.Data.Repository;
using MVCCasino.Models;
using MVCCasino.Models.Responses;

namespace MVCCasino.Services;

public class TransactionService(IWalletRepository walletRepository) : ITransactionService
{
    public void CreateWalletByUserId(string userId)
    {
        var wallet = new Wallet { UserId = userId, CurrentBalance = 0m };
        walletRepository.Create(wallet);
    }

    public DepositResponse ProcessDeposit(string userId, decimal amount)
    {
        try
        {
            walletRepository.Deposit(userId, amount);
            return new DepositResponse { Success = true };
        }
        catch (DbException dbEx)
        {
            Console.Error.WriteLine($"Deposit failed due to a database error: {dbEx.Message}");
            return new DepositResponse
                { Success = false, ErrorMessage = "Deposit failed due to a database error. " + dbEx.Message };
        }
    }

    public WithdrawResponse ProcessWithdraw(string userId, decimal amount)
    {
        try
        {
            walletRepository.Withdraw(userId, amount);
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