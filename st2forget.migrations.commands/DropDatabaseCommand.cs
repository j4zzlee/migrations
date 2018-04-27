using System;
using System.Data.Common;
using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;
using st2forget.console.utils;
using st2forget.utils.commands;
using st2forget.utils.sql;

namespace st2forget.migrations
{
    public class DropDatabaseCommand : Command, IDisposable
    {
        private DbConnection _connection;
        protected string ApplicationPath;

        public DropDatabaseCommand()
        {
            AddArgument("application-path", "a", "Path to application which contains appsettings.json");
        }

        public override string CommandName => "db:drop";
        public override string Description => "Drop database";

        public void Dispose()
        {
            _connection?.Close();
            _connection?.Dispose();
        }

        protected virtual string GetConnectionString()
        {
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
            var builder = new ConfigurationBuilder()
                .SetBasePath(ApplicationPath)
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{environmentName}.json", true)
                .AddEnvironmentVariables();
            var configuration = builder.Build();
            return configuration.GetConnectionString("__MigrationDatabase");
        }

        protected override ICommand Filter()
        {
            ApplicationPath = ReadArgument<string>("application-path") ?? Environment.CurrentDirectory;
            return this;
        }

        protected override void OnExecute()
        {
            
            var conn = GetConnectionString();
            var builder = new SqlConnectionStringBuilder(conn);
            var dataSource = builder.InitialCatalog;
            builder.InitialCatalog = "master";
            _connection = new SqlConnection(builder.ConnectionString);
            
            _connection.Open();
            _connection.Execute($"DROP DATABASE {dataSource}");
            $@"The database {{f:Yellow}}""{dataSource}""{{f:d}} has been dropped.".PrettyPrint(ConsoleColor.Red);
        }
    }
}