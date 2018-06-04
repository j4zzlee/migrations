using System;
using System.IO;
using System.Text.RegularExpressions;
using DateTimeExtensions;
using ConsoleExtensions;
using Commands;

namespace st2forget.migrations
{
    public class GenerateMigrationCommand : DapperCommand
    {
        private string _version;
        private string _migrationPath;
        private string _ticketName;
        public GenerateMigrationCommand() : base()
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
            File.WriteAllText(migrationFile, $@"
-- Name: {_ticketName}
-- Date: {(new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)).AddSeconds(time).ToLocalTime()}
-- Author: {Environment.UserName}
----------------------------
-- Migration up goes here.
----------------------------
--Down--
----------------------------
-- Migration down goes here.
----------------------------
");
            $"[x] Generated {{f:Yellow}}{migrationFile}{{f:d}}".PrettyPrint(ConsoleColor.Green);
        }

        protected override ICommand Filter()
        {
            _version = ReadArgument<string>("version");
            _migrationPath = ReadArgument<string>("migration-path") ?? Path.Combine(Environment.CurrentDirectory, "Migrations");
            _ticketName = ReadArgument<string>("ticket");
            if (!Regex.IsMatch(_ticketName, "^[a-zA-Z][0-9a-zA-Z-_]*", RegexOptions.IgnoreCase | RegexOptions.Multiline))
            {
                throw new Exception("Ticket name could only contains letters & numbers and begin with letters");
            }
            return this;
        }

        public override string CommandName => "db:migrations:generate";
        public override string Description => "Generate migration file based on version provided";
        public override void Dispose()
        {
            
        }
    }
}