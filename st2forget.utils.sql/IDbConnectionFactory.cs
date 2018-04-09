using System;
using System.Data;

namespace st2forget.utils.sql
{
    public interface IDbConnectionFactory : IDisposable
    {
        IDbConnection OpenConnection();
        IDbTransaction OpenTransaction(bool force = false);
        IDbConnection GetConnection();
        IDbTransaction GetTransaction();
        void SetConnection(IDbConnection conn);
    }
}