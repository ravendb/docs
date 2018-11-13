using System;

namespace Raven.Documentation.Parser.Helpers
{
    internal class VersionsParser
    {
        public decimal ToNumber(string version)
        {
            decimal.TryParse(version, out var result);
            return result;
        }

        public string GetMajorVersion(string version) => version.Split('.')[0];

        public bool IsEqualOrLower(string a, string b) => CompareVersions(a, b, (x, y) => x <= y);

        public bool IsGreaterThan(string a, string b) => CompareVersions(a, b, (x, y) => x > y);

        private bool CompareVersions(string aVersion, string bVersion, Func<decimal, decimal, bool> comparer)
        {
            var a = ToNumber(aVersion);
            var b = ToNumber(bVersion);
            return a != 0 && b != 0 && comparer(a, b);
        }
    }
}
