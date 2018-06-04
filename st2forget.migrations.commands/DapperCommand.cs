using Commands;
using System;

namespace st2forget.migrations
{
    public abstract class DapperCommand : Command, IDisposable
    {
        protected DapperCommand()
        {
        }

        public abstract void Dispose();
    }
}