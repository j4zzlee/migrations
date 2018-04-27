using System;
using st2forget.utils.commands;

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