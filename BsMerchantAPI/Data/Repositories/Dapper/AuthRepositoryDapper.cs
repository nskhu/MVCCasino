using System.Data;
using System.Data.Common;
using Dapper;

namespace BsMerchantAPI.Data.Repositories.Dapper;

public class AuthRepositoryDapper(IDbConnection dbConnection) : IAuthRepository
{
    public (string? PrivateToken, int StatusCode) GeneratePrivateToken(string publicToken)
    {
        const string procedureName = "GenerateOrUpdatePrivateTokenProcedure";
        var parameters = new DynamicParameters();
        parameters.Add("@PublicToken", publicToken, DbType.String, ParameterDirection.Input);
        parameters.Add("@PrivateToken", dbType: DbType.String, direction: ParameterDirection.Output);
        parameters.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);

        dbConnection.Execute(procedureName, parameters, commandType: CommandType.StoredProcedure);

        var privateToken = parameters.Get<string?>("@PrivateToken");
        var statusCode = parameters.Get<int>("@StatusCode");

        return (privateToken, statusCode);
    }
}