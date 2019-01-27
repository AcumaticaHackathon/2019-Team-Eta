using System;
using System.Data.SqlClient;
using Serilog;

namespace AcumaticaUniversity.ValidationService.Controllers
{
    public class Database
    {
        private string _connectionString = "Server=.\\EXPRESS2012;Database=master;Trusted_Connection=True;";
        private readonly string databaseName = "AcumaticaDb";
        private readonly SqlConnection masterConnection;

        public Database()
        {
            this.masterConnection = new SqlConnection(_connectionString);
        }

        public void Restore(string path)
        {
            Drop();
            var db = databaseName;
            ExecuteNonQuery(
                string.Format(@"USE MASTER;" +
                              "RESTORE DATABASE [{0}] FROM DISK='{1}' WITH REPLACE; " +
                              "ALTER DATABASE [{0}] SET MULTI_USER;", db, path), masterConnection);
        }

        public void Drop()
        {
            var db = databaseName;
            ExecuteNonQuery("DECLARE @dbname nvarchar(128) SET @dbname = N'" + db + "' " +
                            "IF(EXISTS(SELECT name FROM master.dbo.sysdatabases WHERE('[' + name + ']' = @dbname OR name = @dbname))) BEGIN " +
                            "EXEC msdb.dbo.sp_delete_database_backuphistory @database_name=N'" +
                            db + "';" +
                            "USE [master];" +
                            "ALTER DATABASE [" + db +
                            "] SET  SINGLE_USER WITH ROLLBACK IMMEDIATE;" +
                            "USE [master];" +
                            "DROP DATABASE [" + db + "]; END", masterConnection);
        }

        private void ExecuteNonQuery(string query, SqlConnection connection)
        {
            try
            {
                using (var cmd = new SqlCommand(query, connection))
                {
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                throw;
            }
            finally
            {
                connection.Close();
            }
        }
    }
}