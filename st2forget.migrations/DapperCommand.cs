using System;
using st2forget.utils.commands;

namespace st2forget.migrations
{
    public abstract class DapperCommand : Command, IDisposable
    {
        protected IServiceProvider Container;

        protected DapperCommand(IServiceProvider container)
        {
            Container = container;
        }

        public abstract void Dispose();
    }
}