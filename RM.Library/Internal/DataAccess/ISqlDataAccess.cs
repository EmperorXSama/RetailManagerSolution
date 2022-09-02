namespace RM.Library.Internal.DataAccess;

public interface ISqlDataAccess
{
    List<T> LoadData<T, TU>(string storedProcedure, TU parameter, string connectionStringName);
    Task<int> SaveData<T>(string storedProcedure, T parameter, string connectionStringName);
}