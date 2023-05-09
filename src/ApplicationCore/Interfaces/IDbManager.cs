using System.Data.SqlClient;

namespace ApplicationCore.Interfaces
{
    public interface IDbManager
    {
        public void OpenTransaction();

        public void CommitTransaction();

        public void RollbackTransaction();

        public void OpenConnection();

        public void CloseConnection();

        public SqlCommand CreateCommand();
    }
}
