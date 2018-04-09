using System;
using st2forget.utils.sql;

namespace st2forget.migrations
{
    public class MigrateUpCommand : MigrationCommand
    {
        public MigrateUpCommand(IServiceProvider container, IMigrationExecuter executer) : base(container, executer)
        {
            IsDown = false;
        }

        public override string CommandName => "db:migrate:up";
        public override string Description => "Migrate up database version";
    }
}