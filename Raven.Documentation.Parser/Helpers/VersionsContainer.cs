using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Raven.Documentation.Parser.Helpers
{
    internal class VersionsContainer
    {
        private const string FirstNonLegacyVersion = "4.0";

        private static readonly VersionsParser VersionsParser = new VersionsParser();
        private static readonly decimal FirstAnalyzedVersionNumber = VersionsParser.ToNumber(FirstNonLegacyVersion);

        private readonly List<string> _allVersions;

        public VersionsContainer(string documentationDirectory)
        {
            var directories = Directory.GetDirectories(documentationDirectory);

            _allVersions = directories.Select(x => new DirectoryInfo(x))
                .Select(x => x.Name)
                .ToList();
        }

        public List<string> GetMinorVersionsInRange(string minVersion, string maxVersion)
        {
            if (IsLegacyVersion(minVersion))
                return new List<string> {minVersion};

            var minorVersions = GetNonLegacyVersions();

            var query = minorVersions
                .Where(x => VersionsParser.IsEqualOrLower(minVersion, x));

            if (string.IsNullOrEmpty(maxVersion) == false)
                query = query.Where(x => VersionsParser.IsEqualOrLower(x, maxVersion));

            return query.ToList();
        }

        private bool IsLegacyVersion(string version)
        {
            var versionNumber = VersionsParser.ToNumber(version);
            return versionNumber < FirstAnalyzedVersionNumber;
        }

        private List<string> GetNonLegacyVersions()
        {
            return _allVersions.Where(x => IsLegacyVersion(x) == false).ToList();
        }
    }
}
