using System;

namespace st2forget.migrations
{
    public class MigrateUpCommand : MigrationCommand
    {
        public MigrateUpCommand(IMigrationExecuter executer) : base(executer)
        {
            IsDown = false;
        }

        public override string CommandName => "db:migrate:up";
        public override string Description => "Migrate up database version";
    }
}