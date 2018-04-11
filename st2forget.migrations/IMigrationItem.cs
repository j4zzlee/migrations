using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace st2forget.migrations
{
    public interface IMigrationItem
    {
        void Up();
        void Down();
    }
}
