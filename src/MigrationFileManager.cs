using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace st2forget.migrations
{
    public class MigrationFileManager
    {
        public MigrationFileManager() { }

        public IEnumerable<string> GetAllVersions(string migrationPath, bool isDown)
        {
            var versions = Directory.GetDirectories(migrationPath);
            return isDown 
                ? versions.OrderByDescending(v => v, new VersionComparer())
                : versions.OrderBy(v => v, new VersionComparer());
        }

        public IEnumerable<string> GetMigrations(string migrationPath, string version, bool isDown)
        {
            var versionPath = Path.Combine(migrationPath, version);
            var files = Directory.GetFiles(versionPath);

            return isDown
                ? files.OrderByDescending(f => f, new MigrationFileComparer())
                : files.OrderBy(f => f, new MigrationFileComparer());
        }
    }
}
