using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace RM.Library.Internal.DataAccess;

public class SqlDataAccess : ISqlDataAccess, IDisposable
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
             var output = await connection.ExecuteAsync(storedProcedure, parameter,
                 commandType: CommandType.StoredProcedure);
             return output;
        }
    }
    
    
    // SQL transactions section 
    private IDbConnection _connection;
    private IDbTransaction _transaction;
    private bool _isClosed = false;

    public async Task<int> SaveDataInTransaction<T>(string storedProcedure, T parameter)
    {
        
            var output =  await _connection.ExecuteAsync(storedProcedure, parameter,
                commandType: CommandType.StoredProcedure , transaction:_transaction);

            return output;
    }
    public List<T> LoadDataInTransaction<T, TU>(string storedProcedure, TU parameter)
    {

            List<T> rows = _connection.Query<T>(storedProcedure, parameter,
                commandType: CommandType.StoredProcedure , transaction:_transaction).ToList();

            return rows;

    }
    public void StartTransaction(string connectionStringName )
    {
        // get connection string
        string connectionString = _config.GetConnectionString(connectionStringName);
        // get the connection 
        _connection = new SqlConnection(connectionString);
        // open connection 
        _connection.Open();
        // start transaction 
        _transaction = _connection.BeginTransaction();
        _isClosed = false;
    }
    public void CommitTransaction()
    {
        _transaction?.Commit();
        _connection?.Close();
        _isClosed = true;
    }

    public void RollBack()
    {
        _transaction?.Rollback();
        _connection?.Close();
        _isClosed = true;
    }

    public void Dispose()
    {
        if (_isClosed = false)
        {
            try
            {
                CommitTransaction();
            }
            catch (Exception e)
            {
                //Log An Issue
            }
        }

        _connection = null;
        _transaction = null;
    }
}