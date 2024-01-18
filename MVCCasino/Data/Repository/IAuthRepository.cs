namespace MVCCasino.Data.Repository;

public interface IAuthRepository
{
    string GeneratePublicToken(string userId);
}