using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace RM.Library.Internal.DataAccess;

public class SqlDataAccess : ISqlDataAccess
{
    private readonly IConfiguration _config;

    public SqlDataAccess(IConfiguration config)
    {
        _config = config;
    }
    // dapper read method 
    public List<T> LoadData<T, TU>(string storedProcedure, TU parameter, string connectionStringName)
    {
        string connectionString = _config.GetConnectionString(connectionStringName);

        using (IDbConnection connection = new SqlConnection(connectionString))
        {
            List<T> rows = connection.Query<T>(storedProcedure, parameter,
                commandType: CommandType.StoredProcedure).ToList();

            return rows;
        }
    }
    
    // dapper write data 
    public async Task<int> SaveData<T>(string storedProcedure, T parameter, string connectionStringName)
    {
        string connectionString = _config.GetConnectionString(connectionStringName);

        using (IDbConnection connection = new SqlConnection(connectionString))
        {
             var output = connection.Execute(storedProcedure, parameter,
                commandType: CommandType.StoredProcedure);
             return output;
        }
    }
}