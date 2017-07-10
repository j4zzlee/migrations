using System;
using st2forget.utils.sql;

namespace st2forget.migrations
{
    public class MigrateDownCommand : MigrationCommand
    {
        public MigrateDownCommand(IServiceProvider container, IMigrationExecuter executer) : base(container, executer)
        {
            IsDown = true;
        }

        public override string CommandName => "db:migrate:down";
        public override string Description => "Migrate down database version";
    }
}