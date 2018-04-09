using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using st2forget.utils.commands;
using st2forget.utils.sql;
using Xunit;
using Dapper;

namespace st2forget.migrations.Tests
{
    public class MigrateTest
    {
        private readonly IServiceProvider _provider;
        private readonly IConfigurationRoot _configurationRoot;
        private readonly IServiceCollection _serviceCollection;
        public MigrateTest()
        {
            _serviceCollection = new ServiceCollection();
            _serviceCollection.AddOptions();

            _serviceCollection.AddScoped<IDbConnectionFactory, DbConnectionFactory>();
            _serviceCollection.AddScoped<IMigrationExecuter, SqlMigrationExecuter>();
            _serviceCollection.AddScoped<ICommand, MigrateUpCommand>();
            _serviceCollection.AddScoped<ICommand, MigrateDownCommand>();
            _serviceCollection.AddScoped(provider => new HelpListCommand(provider.GetServices<ICommand>().ToList()));

            _provider = _serviceCollection.BuildServiceProvider();
        }

        public static string MigrationTestDirectory = Path.Combine(
            AppContext.BaseDirectory,
            "MigrationTestCases");

        public static IEnumerable<object[]> MigrateNoTicketSuccessTestCases => new[]
        {
            new [] { new []
            {
                "-v:1.0.1", $"-p:{Path.Combine(MigrationTestDirectory, "Up", "NoTicket")}" // , "-t:TestMigration"
            }} 
        };
//
//        [Theory]
//        [MemberData(nameof(MigrateNoTicketSuccessTestCases))]
//        public void MigrateUpNoTicketSuccessTest(string[] args)
//        {
//            using (var scope = _provider.CreateScope())
//            {
//                var provider = scope.ServiceProvider;
//                var command = provider
//                    .GetServices<ICommand>()
//                    .FirstOrDefault(r => r.CommandName.Equals("db:migrate:up"));
//                command.ReadArguments(args);
//                command.Execute();
//            }
//            using (var conn = new SqlConnection(_configurationRoot.GetConnectionString("MigrationDatabase")))
//            {
//                conn.Open();
//                var exists = conn.Query<bool>(@"
//SELECT ISNULL(
//    (
//        SELECT TOP 1 1 
//        FROM INFORMATION_SCHEMA.TABLES 
//        WHERE TABLE_SCHEMA = 'dbo' 
//        AND (
//            TABLE_NAME = 'User'
//            OR TABLE_NAME = 'AspNetRoles'
//        )
//    ), 0)
//").FirstOrDefault();
//                Assert.True(exists);
//            }
//        }
//
//        [Theory]
//        [MemberData(nameof(MigrateNoTicketSuccessTestCases))]
//        public void MigrateDownNoTicketSuccessTest(string[] args)
//        {
//            using (var scope = _provider.CreateScope())
//            {
//                var provider = scope.ServiceProvider;
//                var command = provider
//                    .GetServices<ICommand>()
//                    .FirstOrDefault(r => r.CommandName.Equals("db:migrate:down"));
//                command.ReadArguments(args);
//                command.Execute();
//            }
//            using (var conn = new SqlConnection(_configurationRoot.GetConnectionString("MigrationDatabase")))
//            {
//                conn.Open();
//                Assert.False(conn.Query<bool>(@"
//SELECT ISNULL(
//    (
//        SELECT TOP 1 1 
//        FROM INFORMATION_SCHEMA.TABLES 
//        WHERE TABLE_SCHEMA = 'dbo' 
//        AND (
//            TABLE_NAME = 'User'
//            OR TABLE_NAME = 'AspNetRoles'
//        )
//    ), 0)
//").FirstOrDefault());
//            }
//        }
    }
}
