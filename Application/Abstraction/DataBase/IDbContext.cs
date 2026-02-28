using Dapper;
using System.Data;

namespace Application.Abstraction.DataBase
{
    public interface IDbContext
    {
        Task<TEntity> ExecuteStoredProcedure<TEntity>(string storedProcedureName, params object[] parameters);
        Task<IList<TEntity>> ExecuteStoredProcedureList<TEntity>(string storedProcedureName, params object[] parameters);
        Task<bool> ExecuteStoredProcedure(string storedProcedureName, params object[] parameters);
        IDbConnection GetDbConnection();
        DynamicParameters GetDapperDynamicParameters(params object[] parameters);
    }
}
