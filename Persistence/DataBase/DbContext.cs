using Application.Abstraction.DataBase;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Persistence.DataBase
{
    public class DbContext : IDbContext
    {
        private readonly string _connectionString;
        public DbContext(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ECommerce");
        }

        public async Task<TEntity> ExecuteStoredProcedure<TEntity>(string storedProcedureName, params object[] parameters)
        {
            using IDbConnection db = new SqlConnection(_connectionString);
            var entity = await db.QueryAsync<TEntity>(storedProcedureName, GetDapperDynamicParameters(parameters), commandType: CommandType.StoredProcedure, commandTimeout: 0);
            return entity.FirstOrDefault();
        }

        public async Task<IList<TEntity>> ExecuteStoredProcedureList<TEntity>(string storedProcedureName, params object[] parameters )
        {
            using IDbConnection db = new SqlConnection(_connectionString);
            var entities = await db.QueryAsync<TEntity>(storedProcedureName, GetDapperDynamicParameters(parameters), commandType: CommandType.StoredProcedure, commandTimeout: 0);            
            return entities.ToList();
        }

        public async Task<IList<TEntity>> ExecuteStoredProcedureList<TEntity>(string storedProcedureName)
        {
            using IDbConnection db = new SqlConnection(_connectionString);
            var entities = await db.QueryAsync<TEntity>(storedProcedureName, null, commandType: CommandType.StoredProcedure, commandTimeout: 0);
            return entities.ToList();
        }

        public async Task<bool> ExecuteStoredProcedure(string storedProcedureName, params object[] parameters)
        {
            bool isRecordInserted = false;
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                await db.QueryAsync(storedProcedureName, GetDapperDynamicParameters(parameters), commandType: CommandType.StoredProcedure, commandTimeout: 0);
                isRecordInserted = true;
            }
            return isRecordInserted;
        }
        public DynamicParameters GetDapperDynamicParameters(params object[] parameters)
        {
            var dynamicParams = new DynamicParameters();
            for (int index = 0; index <= parameters.Length - 1; index++)
            {
                var item = ((SqlParameter)parameters[index]);
                dynamicParams.Add(item.ParameterName, item.Value, item.DbType, item.Direction);
            }
            return dynamicParams;
        }
        public IDbConnection GetDbConnection()
        {
            return new SqlConnection(_connectionString);
        }

       
    }
}
