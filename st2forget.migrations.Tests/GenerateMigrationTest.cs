using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using st2forget.utils.commands;
using st2forget.utils.sql;
using Xunit;

namespace st2forget.migrations.Tests
{
    public class GenerateMigrationTest
    {
        private readonly IServiceProvider _provider;
        private readonly IConfigurationRoot _configurationRoot;
        private readonly IServiceCollection _serviceCollection;
        public GenerateMigrationTest()
        {
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var builder = new ConfigurationBuilder()
                .SetBasePath(
                    AppContext.BaseDirectory.Substring(
                        0,
                        AppContext.BaseDirectory.IndexOf("bin", StringComparison.Ordinal)
                    )
                )
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{environmentName}.json", true)
                .AddEnvironmentVariables();
            _configurationRoot = builder.Build();

            _serviceCollection = new ServiceCollection();
            _serviceCollection.AddOptions();
            //_serviceCollection.Configure<ConnectionSettings>(connectionSettings =>
            //{
            //    connectionSettings.ConnectionString = _configurationRoot.GetConnectionString("MigrationDatabase");
            //});

            _serviceCollection.AddScoped<IDbConnectionFactory, DbConnectionFactory>();
            _serviceCollection.AddScoped<GenerateMigrationCommand>();
            _serviceCollection.AddScoped(provider => new HelpListCommand(provider.GetServices<ICommand>().Where(c => !c.CommandName.Equals("commands:list")).ToList()));

            _provider = _serviceCollection.BuildServiceProvider();
        }

        public static IEnumerable<object[]> MissingArgumentsTestCases => new[]
        {
            // Missing Tickets
            new[] {new[] {"-v"}},
            new[] {new[] {"-version"}},

            // Missing Version
            new[] {new[] {"-t"}},
            new[] {new[] {"-ticket"}},

            // Wrong syntax
            new[] {new[] {"-v", "--t"}},
            new[] {new[] {"-v", "-ticket"}},
            new[] {new[] {"--version", "--t"}},
            new[] {new[] {"--version", "-ticket"}},

            new[] {new[] {"--v", "-t"}},
            new[] {new[] {"--v", "--ticket"}},
            new[] {new[] {"-version", "-t"}},
            new[] {new[] {"-version", "--ticket"}},

            new[] {new[] {"--v", "--t"}},
            new[] {new[] {"--v", "-ticket"}},
            new[] {new[] {"-version", "--t"}},
            new[] {new[] {"-version", "-ticket"}},
        };

        [Theory]
        [MemberData(nameof(MissingArgumentsTestCases))]
        public void MissingArguments(string[] args)
        {
            var command = _provider.GetService<GenerateMigrationCommand>();
            Assert.Throws<ArgumentException>(() => command.ReadArguments(args));
        }

        public static string MigrationTestDirectory = Path.Combine(
            AppContext.BaseDirectory,
            "GenerateMigrationTest");

        public static IEnumerable<object[]> ShouldCreateMigrationVersionDirectoryTestCases => new[]
        {
            new [] {"-v:1.0.1", $"-p:{MigrationTestDirectory}", "-t:TestMigration"},
            new [] {"-v:1.0.2", $"-p:{MigrationTestDirectory}", "-t:TestMigration3"}
        };

        [Theory]
        [MemberData(nameof(ShouldCreateMigrationVersionDirectoryTestCases))]
        public void ShouldCreateMigrationVersionDirectory(string version, string migrationPath, string ticket)
        {
            // 
            if (Directory.Exists(MigrationTestDirectory))
            {
                Directory.Delete(MigrationTestDirectory, true);
            }

            Directory.CreateDirectory(MigrationTestDirectory);

            var command = _provider.GetService<GenerateMigrationCommand>();
            command.ReadArguments(new []
            {
                version, migrationPath, ticket
            });
            command.Execute();
            var versionDir = Path.Combine(MigrationTestDirectory, version.Split(':')[1]);
            Assert.True(Directory.Exists(versionDir));
            Assert.True(Directory.EnumerateFiles(versionDir).Count() == 1);
            Directory.Delete(MigrationTestDirectory, true);
        }
    }
}
