using System;

namespace st2forget.migrations
{
    public interface IMigrationExecuter: IDisposable
    {
        void Init();
        Migration GetLastExecutedMigration();
        bool IsExecuted(string migration);
        void ExecuteMigration(string path, bool isDown);
    }
}