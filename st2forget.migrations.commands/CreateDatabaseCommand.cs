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
    public class CreateDatabaseCommand : Command, IDisposable
    {
        private readonly IMigrationExecuter _executer;
        private DbConnection _connection;
        protected string ApplicationPath;

        public CreateDatabaseCommand(IMigrationExecuter executer)
        {
            AddArgument("application-path", "a", "Path to application which contains appsettings.json");
            _executer = executer;
        }

        public override string CommandName => "db:create";
        public override string Description => "Create new database";

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
            _connection.Execute($"CREATE DATABASE {dataSource}");
            $@"The database {{f:Yellow}}""{dataSource}""{{f:d}} has been created successfully.".PrettyPrint(ConsoleColor.Green);

            _executer.SetConnectionString(conn);
            _executer.Init();
        }
    }
}