using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using Dapper;
using Microsoft.Extensions.Options;
using st2forget.commons.datetime;
using st2forget.console.utils;
using st2forget.utils.sql;

namespace st2forget.migrations
{
    public class SqlMigrationExecuter: IMigrationExecuter
    {
        private IDbConnection _sqlConnection;
        public SqlMigrationExecuter()
        {
        }

        private bool IsInitialized()
        {
            return _sqlConnection
                .QueryFirstOrDefault<bool>($@"
SELECT CASE 
    WHEN EXISTS(
        SELECT * 
                 FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = 'dbo' 
                 AND  TABLE_NAME = '{nameof(Migration)}'
    )
    THEN (
        SELECT 1
    ) 
    ELSE 0
END
");
        }

        public void Init()
        {
            if (IsInitialized())
            {
                return;
            }
            _sqlConnection
                .Execute(@"
CREATE TABLE [dbo].[Migration](
	[Name] [varchar](50) NOT NULL,
	[LastRun] [int] NOT NULL
) ON [PRIMARY]
");
        }

        public void DeleteMigration(string name, IDbTransaction transaction)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Migration name is required.");
            }

            _sqlConnection
                .Execute($@"
DELETE FROM [dbo].[{nameof(Migration)}]
WHERE Name = @Name
", new Migration
                {
                    Name = name
                }, transaction: transaction);
        }

        public void AddMigration(string name, IDbTransaction transaction)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Migration name is required.");
            }

            _sqlConnection
                .Execute($@"
INSERT INTO [dbo].[{nameof(Migration)}] ({typeof(Migration).GetDatabaseSchemas()}) 
VALUES ({typeof(Migration).GetDatabaseValueSchemas()})
", new Migration
                {
                    Name = name,
                    LastRun = DateTime.Now.ToUnixTimestamp()
                }, transaction: transaction);
        }

        public Migration GetLastExecutedMigration()
        {
            return _sqlConnection
                .QueryFirstOrDefault<Migration>($@"
SELECT TOP 1 * FROM [dbo].[{nameof(Migration)}] ORDER BY LastRun DESC, Name DESC
");
        }

        public bool IsExecuted(string migration)
        {
            return _sqlConnection
                       .QueryFirstOrDefault<Migration>($@"
SELECT TOP 1 * FROM [dbo].[{nameof(Migration)}] 
WHERE [Name] = @Name
",
                           new
                           {
                               Name = migration
                           }) != null;
        }


        public void ExecuteMigration(string file, bool isDown)
        {
            using (var transaction = _sqlConnection.BeginTransaction())
            {
                try
                {
                    var sql = File.ReadAllText(file);
                    var sqls = sql.Split(new[] { "--Down--" }, StringSplitOptions.None);
                    sql = isDown ? (sqls.Length > 1 ? sqls[1] : string.Empty) : sqls[0];
                    sql = sql.Trim();

                    if (string.IsNullOrWhiteSpace(sql))
                    {
                        "[x] Warning: No migration script executed.".PrettyPrint(ConsoleColor.DarkYellow);
                        return;
                    }

                    sql.PrettyPrint(ConsoleColor.Gray);

                    _sqlConnection.Execute(
                        sql,
                        transaction: transaction
                    );

                    "*** Done\r\n".PrettyPrint(ConsoleColor.Green);

                    // Track it
                    if (isDown)
                    {
                        DeleteMigration(Path.GetFileName(file), transaction);
                    }
                    else
                    {
                        AddMigration(Path.GetFileName(file), transaction);
                    }
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
        
        public void Dispose()
        {
            _sqlConnection?.Close();
        }

        public IMigrationExecuter SetConnectionString(string conn)
        {
            _sqlConnection = new SqlConnection(conn);
            _sqlConnection.Open();
            return this;
        }
    }
}
