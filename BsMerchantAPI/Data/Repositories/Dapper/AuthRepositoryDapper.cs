using System.Data;
using Dapper;

namespace BsMerchantAPI.Data.Repositories.Dapper;

public class AuthRepositoryDapper(IDbConnection dbConnection) : IAuthRepository
{
    public string GeneratePrivateToken(string publicToken)
    {
        const string procedureName = "GenerateOrUpdatePrivateTokenProcedure";
        var parameters = new { PublicToken = publicToken };
        var privateToken =
            dbConnection.QueryFirstOrDefault<string>(procedureName, parameters,
                commandType: CommandType.StoredProcedure);

        return privateToken;
    }
}