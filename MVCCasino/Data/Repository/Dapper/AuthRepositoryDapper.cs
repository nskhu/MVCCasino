using System.Data;
using System.Data.Common;
using Dapper;

namespace MVCCasino.Data.Repository.Dapper;

public class AuthRepositoryDapper(IDbConnection dbConnection) : IAuthRepository
{
    public string GeneratePublicToken(string userId)
    {
        const string procedureName = "GenerateOrUpdatePublicTokenProcedure";
        var parameters = new { UserId = userId };
        var publicToken =
            dbConnection.QueryFirstOrDefault<string>(procedureName, parameters,
                commandType: CommandType.StoredProcedure);

        return publicToken;
    }
}