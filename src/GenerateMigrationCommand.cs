using System;
using System.IO;
using System.Text.RegularExpressions;
using st2forget.commons.datetime;
using st2forget.console.utils;
using st2forget.utils.commands;
using st2forget.utils.sql;

namespace st2forget.migrations
{
    public class GenerateMigrationCommand : DapperCommand
    {
        private string _version;
        private string _migrationPath;
        private string _ticketName;
        public GenerateMigrationCommand(IServiceProvider container) : base(container)
        {
            AddArgument("version", "v", "Version of project. e.g: 2.0.1", true, false, "^\\d+(\\.\\d+)*$");
            AddArgument("migration-path", "p", "Path of migration folder. e.g: C:\\Users\\consoto\\Migrations");
            AddArgument("ticket", "t", "Ticket name. e.g: STF_111", true);
        }

        protected override void OnExecute()
        {
            if (!Directory.Exists(_migrationPath))
            {
                throw new DirectoryNotFoundException($"Directory {_migrationPath} is not found.");
            }
            
            var migrationPath = Path.Combine(_migrationPath, _version);

            if (!Directory.Exists(migrationPath))
            {
                Directory.CreateDirectory(migrationPath);
            }

            var time = DateTime.Now.ToUnixTimestamp();
            var migrationFile = Path.Combine(migrationPath, $"{time}-{_ticketName}.sql");
            File.Create(migrationFile).Dispose();
            $"[x] Generated {{f:Yellow}}{migrationFile}{{f:d}}".PrettyPrint(ConsoleColor.Green);
        }

        protected override ICommand Filter()
        {
            _version = ReadArgument<string>("version");

            _migrationPath = ReadArgument<string>("migration-path") ?? Path.Combine(
                AppContext.BaseDirectory.Substring(
                    0,
                    AppContext.BaseDirectory.IndexOf("bin", StringComparison.Ordinal)),
                "Migrations");
            _ticketName = ReadArgument<string>("ticket");
            return this;
        }

        public override string CommandName => "db:migrations:generate";
        public override string Description => "Generate migration file based on version provided";
        public override void Dispose()
        {
            
        }
    }
}