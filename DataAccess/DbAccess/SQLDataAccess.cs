using Microsoft.Extensions.Configuration;
using System.Data;
using Dapper;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess.DbAccess
{
    public class SQLDataAccess : ISQLDataAccess
    {
        private readonly IConfiguration _config;

        public SQLDataAccess(IConfiguration config)
        {
            _config = config;
        }

        public async Task<IEnumerable<T>> LoadData<T>(string sql, string connectionId = "DefaultConnection")
        {
            using IDbConnection connection = new OracleConnection(_config.GetConnectionString(connectionId));
            return await connection.QueryAsync<T>(sql);
        }

        public async Task<IEnumerable<T>> LoadData<T, U>(string sql, U data, string connectionId = "DefaultConnection")
        {
            using IDbConnection connection = new OracleConnection(_config.GetConnectionString(connectionId));
            return await connection.QueryAsync<T>(sql, data);
        }

        public async Task ExecuteProcedure(string sql, string connectionId = "DefaultConnection")
        {
            using IDbConnection connection = new OracleConnection(_config.GetConnectionString(connectionId));
            await connection.QueryAsync(sql);
        }

        public async Task<IEnumerable<T>> ExecuteProcedure<T>(string sql, string connectionId = "DefaultConnection")
        {
            using IDbConnection connection = new OracleConnection(_config.GetConnectionString(connectionId));
            return await connection.QueryAsync<T>(sql);
        }

        public async Task SaveData<T>(string sql, T data, string connectionId = "DefaultConnection")
        {
            using IDbConnection connection = new OracleConnection(_config.GetConnectionString(connectionId));
            await connection.ExecuteAsync(sql, data);
        }
    }
}
