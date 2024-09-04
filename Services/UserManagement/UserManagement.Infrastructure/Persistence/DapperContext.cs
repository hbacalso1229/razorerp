using Microsoft.Data.SqlClient;
using System.Data;
using UserManagement.Application.Common.Interfaces;

namespace UserManagement.Infrastructure.Persistence
{
    public class DapperContext : IDapperContext
    {
        private readonly string _connectionString;

        public DapperContext(string connectionString)
        {
            ArgumentNullException.ThrowIfNullOrEmpty(nameof(connectionString));           

            _connectionString = connectionString;  
        }

        public async Task<M> QueryCustom<M>(Func<IDbConnection, Task<M>> getData)
        {
            return await ExecuteDbConnectionAsync<M>(getData);
        }

        private async Task<M> ExecuteDbConnectionAsync<M>(Func<IDbConnection, Task<M>> getData)
        {
            try
            {
                using (IDbConnection dbConnection = new SqlConnection(_connectionString))
                {
                    return await getData(dbConnection);
                }                        
            }                
            catch (Exception ex)
            {
                throw;
            }
        }      
    }   
}
