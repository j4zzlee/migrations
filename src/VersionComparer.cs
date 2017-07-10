using System;
using System.Collections.Generic;

namespace st2forget.migrations
{
    public class VersionComparer : IComparer<string>
    {
        public int Compare(string v1, string v2)
        {
            if (string.IsNullOrWhiteSpace(v1))
            {
                return -1;
            }
            if (string.IsNullOrWhiteSpace(v2))
            {
                return 1;
            }
            var oldVersions = v1.Split('.');
            var newVersions = v2.Split('.');

            // oldVersions: 8.0.1, newVersions: 8.0 => [8.0, 8.0.1]
            if (oldVersions.Length >= newVersions.Length)
            {
                for (var i = 0; i < newVersions.Length; i++)
                {
                    var oldVersion = oldVersions[i];
                    var newVersion = newVersions[i];
                    if (Convert.ToInt32(oldVersion) > Convert.ToInt32(newVersion))
                    {
                        return 1;
                    }
                    if (Convert.ToInt32(newVersion) > Convert.ToInt32(oldVersion))
                    {
                        return -1;
                    }
                }
                return 1;
            }

            for (var i = 0; i < oldVersions.Length; i++)
            {
                var oldVersion = oldVersions[i];
                var newVersion = newVersions[i];
                if (Convert.ToInt32(oldVersion) > Convert.ToInt32(newVersion))
                {
                    return 1;
                }
                if (Convert.ToInt32(newVersion) > Convert.ToInt32(oldVersion))
                {
                    return -1;
                }
            }
            return -1;
        }
    }
}