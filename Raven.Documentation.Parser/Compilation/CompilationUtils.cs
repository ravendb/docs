using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Raven.Documentation.Parser.Data;
using Raven.Documentation.Parser.Helpers;

namespace Raven.Documentation.Parser.Compilation
{
    public class CompilationUtils
    {
        public class Parameters
        {
            public FileInfo File { get; set; }
            public FolderItem Page { get; set; }
            public string DocumentationVersion { get; set; }
            public string SourceDocumentationVersion { get; set; }
            public List<DocumentationMapping> Mappings { get; set; }
        }

        internal class Context
        {
            private readonly VersionsParser _versionsParser = new VersionsParser();

            private readonly Dictionary<LastSupportedVersionKey, string> _lastSupportedVersions =
                new Dictionary<LastSupportedVersionKey, string>(new KeyComparer());

            public void RegisterCompilation(RegistrationInput input)
            {
                var majorVersion = _versionsParser.GetMajorVersion(input.DocumentationVersion);
                var key = GetKey(input, majorVersion);

                if (_lastSupportedVersions.ContainsKey(key))
                {
                    ValidateLastSupportedVersion(input, key);
                    UpdateLastSupportedVersion(input, key);
                }
                else
                {
                    _lastSupportedVersions[key] = input.LastSupportedVersion;
                }
            }

            private void ValidateLastSupportedVersion(RegistrationInput input, LastSupportedVersionKey key)
            {
                var registeredVersion = _lastSupportedVersions[key];

                if (string.IsNullOrEmpty(registeredVersion))
                    return;

                ValidateLastSupportedVersion(input, registeredVersion, input.DocumentationVersion);

                foreach (var supportedVersion in input.SupportedVersions)
                {
                    ValidateLastSupportedVersion(input, registeredVersion, supportedVersion);
                }
            }

            private void ValidateLastSupportedVersion(RegistrationInput input, string registeredVersion, string versionToTest)
            {
                var exceedsLastSupportedVersion = _versionsParser.IsGreaterThan(versionToTest, registeredVersion);

                if (exceedsLastSupportedVersion)
                {
                    var message = GetExceptionMessage(input, registeredVersion);
                    throw new InvalidOperationException(message);
                }
            }

            private void UpdateLastSupportedVersion(RegistrationInput input, LastSupportedVersionKey key)
            {
                var versionToSet = input.LastSupportedVersion;

                if (string.IsNullOrEmpty(versionToSet))
                    return;

                var registeredVersion = _lastSupportedVersions[key];

                if (string.IsNullOrEmpty(registeredVersion) == false && _versionsParser.IsGreaterThan(registeredVersion, versionToSet))
                    return;

                _lastSupportedVersions[key] = versionToSet;
            }

            private string GetExceptionMessage(RegistrationInput input, string lastSupportedVersion)
            {
                var builder = new StringBuilder($"The document {input.Key} [{input.Language}] has ");
                builder.Append($"{nameof(RegistrationInput.LastSupportedVersion)} set to {lastSupportedVersion} ");
                builder.Append($"but its definition was also included in .docs.json for {input.DocumentationVersion}. ");
                builder.Append($"Please either remove the {nameof(RegistrationInput.LastSupportedVersion)} field ");
                builder.Append($"or the .docs.json entry from {input.DocumentationVersion}.");
                return builder.ToString();
            }

            private LastSupportedVersionKey GetKey(RegistrationInput input, string majorVersion) =>
                new LastSupportedVersionKey
                {
                    Language = input.Language,
                    Key = input.Key,
                    MajorVersion = majorVersion
                };

            public class RegistrationInput
            {
                public string Key { get; set; }
                public Language Language { get; set; }
                public string DocumentationVersion { get; set; }
                public List<string> SupportedVersions { get; set; }
                public string LastSupportedVersion { get; set; }
            }

            private class LastSupportedVersionKey
            {
                public string Key { get; set; }
                public Language Language { get; set; }
                public string MajorVersion { get; set; }
            }

            private class KeyComparer : IEqualityComparer<LastSupportedVersionKey>
            {
                public bool Equals(LastSupportedVersionKey x, LastSupportedVersionKey y)
                {
                    if (x == null && y == null)
                        return true;

                    if (x == null || y == null)
                        return false;

                    return string.Equals(x.Key, y.Key, StringComparison.OrdinalIgnoreCase)
                           && string.Equals(x.MajorVersion, y.MajorVersion, StringComparison.OrdinalIgnoreCase)
                           && x.Language == y.Language;
                }

                public int GetHashCode(LastSupportedVersionKey obj)
                {
                    return $"{obj.Language}_{obj.Key}_{obj.MajorVersion}".GetHashCode();
                }
            }
        }
    }
}
