using System;
using System.IO;
using System.Threading;
using Microsoft.Extensions.Configuration;
using st2forget.console.utils;
using st2forget.utils.commands;

namespace st2forget.migrations
{
    public abstract class MigrationCommand : DapperCommand
    {
        protected string MigrationPath;
        protected string ApplicationPath;
        protected string Ticket;
        protected bool IsDown;
        protected IMigrationExecuter Executer;

        protected MigrationCommand(IMigrationExecuter executer) : base()
        {
            AddArgument("application-path", "a", "Path to application which contains appsettings.json", true);
            AddArgument("migration-path", "p", "Migration path");
            AddArgument("ticket", "t", "Ticket name", false, false, "\\d+\\-\\w+");

            Executer = executer;
        }

        protected override ICommand Filter()
        {
            Ticket = ReadArgument<string>("ticket");
            ApplicationPath = ReadArgument<string>("application-path");
            MigrationPath = ReadArgument<string>("migration-path") ?? Path.Combine(
                                AppContext.BaseDirectory,
                                "Migrations");
            return this;
        }

        protected virtual string GetConnectionString()
        {
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var builder = new ConfigurationBuilder()
                .SetBasePath(ApplicationPath)
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{environmentName}.json", true)
                .AddEnvironmentVariables();
            var configuration = builder.Build();
            return configuration.GetConnectionString("MigrationDatabase");
        }

        protected override void OnExecute()
        {
            Executer.SetConnectionString(GetConnectionString());
            Executer.Init();
            var migrationFileManager = new MigrationFileManager();
            var hasMigration = false;
            // Find Name Versions
            var versions = migrationFileManager.GetAllVersions(MigrationPath, IsDown);
            foreach (var version in versions)
            {
                var files = migrationFileManager.GetMigrations(MigrationPath, version, IsDown);
                foreach (var file in files)
                {
                    if (!CanRun(file))
                    {
                        continue;
                    }

                    $"[x] Executing migration: {{f:Yellow}}{file}{{f:d}}, version: {{f:Yellow}}{version}{{f:d}}".PrettyPrint(ConsoleColor.Green);
                    Executer.ExecuteMigration(file, IsDown);
                    hasMigration = true;
                    Thread.Sleep(1000);
                }
            }

            if (!hasMigration)
            {
                "[x] No migration executed.".PrettyPrint(ConsoleColor.Yellow);
            }
        }

        private bool CanRun(string file)
        {
            var migrationName = Path.GetFileName(file);
            if (string.IsNullOrWhiteSpace(Ticket))
            {
                return IsDown
                    ? Executer.IsExecuted(migrationName)
                    : !Executer.IsExecuted(migrationName);
            }
            var fileTs = long.Parse(migrationName.Split('-')[0]);
            var ticketTs = long.Parse(Ticket.Split('-')[0]);
            return IsDown
                ? fileTs >= ticketTs && Executer.IsExecuted(migrationName)
                : fileTs <= ticketTs && !Executer.IsExecuted(migrationName);
        }

        public override void Dispose()
        {
            Executer?.Dispose();
        }
    }
}