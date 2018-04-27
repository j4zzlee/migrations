using System;
using System.Collections.Generic;
using System.IO;

namespace st2forget.migrations
{
    public class MigrationFileComparer : IComparer<string>
    {
        public int Compare(string f1, string f2)
        {
            if (f1 == f2)
            {
                return 0;
            }
            if (string.IsNullOrWhiteSpace(f1) || string.IsNullOrWhiteSpace(f2))
            {
                throw new ArgumentNullException($"Must provide not empty migration file");
            }
            var f1Filename = Path.GetFileName(f1);
            var f2FileName = Path.GetFileName(f2);
            var f1Parts = f1Filename.Split('-');
            var f2Parts = f2FileName.Split('-');

            if (f1Parts.Length < 1 || f2Parts.Length < 1)
            {
                throw new ArgumentNullException($"Migration file does not in correct format {{<ts>-<name>.sql}}");
            }

            var canParseF1 = long.TryParse(f1Parts[0], out long ts1);
            var canParseF2 = long.TryParse(f2Parts[0], out long ts2);

            if (!canParseF1 || !canParseF2)
            {
                throw new ArgumentException("First part of migration file's name must be number");
            }

            if (ts1.Equals(ts2))
            {
                throw new ArgumentException($"There should not have 2 file which have same timeline {ts1}");
            }

            return ts1 > ts2 ? 1 : -1;
        }
    }
}