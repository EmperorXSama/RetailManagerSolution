namespace RM.Library.Internal.DataAccess;

public interface ISqlDataAccess : IDisposable
{
    List<T> LoadData<T, TU>(string storedProcedure, TU parameter, string connectionStringName);
    Task<int> SaveData<T>(string storedProcedure, T parameter, string connectionStringName);
    Task<int> SaveDataInTransaction<T>(string storedProcedure, T parameter);
    List<T> LoadDataInTransaction<T, TU>(string storedProcedure, TU parameter);
    void StartTransaction(string connectionStringName);
    void CommitTransaction();
    void RollBack();
}