using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace st2forget.utils.sql
{
    public class DbConnectionFactory : IDbConnectionFactory
    {
        private IDbConnection _connection;
        private IDbTransaction _transaction;
        public DbConnectionFactory(IOptions<ConnectionSettings> options)
        {
            var settings = options.Value;
            _connection = new SqlConnection(settings.ConnectionString);
        }

        public IDbConnection OpenConnection()
        {
            if (_connection.State != ConnectionState.Open)
            {
                _connection.Open();
            }
            return _connection;
        }

        public IDbTransaction OpenTransaction(bool force = false)
        {
            if (!force)
            {
                if (_transaction != null)
                {
                    return _transaction;
                }
            }

            if (_connection.State != ConnectionState.Open)
            {
                _connection.Open();
            }
            _transaction = _connection.BeginTransaction();
            return _transaction;
        }

        public IDbTransaction GetTransaction()
        {
            return _transaction;
        }

        public void SetConnection(IDbConnection conn)
        {
            _transaction?.Dispose();
            _connection?.Dispose();

            _connection = conn;
            if (_connection.State != ConnectionState.Open)
            {
                _connection.Open();
            }
        }

        public IDbConnection GetConnection()
        {
            return _connection;
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _connection?.Dispose();
            _transaction = null;
        }
    }
}