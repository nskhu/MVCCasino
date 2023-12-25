using MVCCasino.Data.Repository;
using MVCCasino.Models;

namespace MVCCasino.Services;

public class WalletService : IWalletService
{
    private readonly IWalletRepository _walletRepository;

    public WalletService(IWalletRepository walletRepository)
    {
        _walletRepository = walletRepository ?? throw new ArgumentNullException(nameof(walletRepository));
    }

    public void CreateWalletByUserId(string userId)
    {
        // Perform any additional logic if needed
        var wallet = new Wallet { UserId = userId, CurrentBalance = 0m };
        _walletRepository.Create(wallet);
    }
}