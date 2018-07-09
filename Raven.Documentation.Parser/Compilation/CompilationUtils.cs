using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Raven.Documentation.Parser.Data;

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
            private readonly HashSet<CompiledEntry> _compiled = new HashSet<CompiledEntry>(new CompiledEntryEqualityComparer());

            public void RegisterCompilation(string key, Language language, string baseVersion, List<string> supportedVersions)
            {
                RegisterBaseVersionCompilation(key, language, baseVersion, supportedVersions?.Any() ?? false);

                if (supportedVersions == null)
                    return;

                foreach (var version in supportedVersions)
                {
                    if (string.Equals(version, baseVersion))
                        continue;

                    RegisterCompilation(key, language, version);
                }
            }

            private void RegisterBaseVersionCompilation(string key, Language language, string version,
                bool hasSupportedVersions)
            {
                var entry = CompiledEntry.New(key, language, version);

                if (hasSupportedVersions && _compiled.Contains(entry))
                    throw new InvalidOperationException($"The document '{key}' has already been compiled for {language} {version}");

                _compiled.Add(entry);
            }

            private void RegisterCompilation(string key, Language language, string version)
            {
                var entry = CompiledEntry.New(key, language, version);

                if (_compiled.Contains(entry))
                    throw new InvalidOperationException($"The document '{key}' has already been compiled for {language} {version}");

                _compiled.Add(entry);
            }

            private class CompiledEntry
            {
                public string Key { get; private set; }

                public Language Language { get; private set; }

                public string Version { get; private set; }

                public static CompiledEntry New(string key, Language language, string version)
                {
                    return new CompiledEntry
                    {
                        Key = key,
                        Language = language,
                        Version = version
                    };
                }
            }

            private class CompiledEntryEqualityComparer : IEqualityComparer<CompiledEntry>
            {
                public bool Equals(CompiledEntry x, CompiledEntry y)
                {
                    if (x == null && y == null)
                        return true;

                    if (x == null || y == null)
                        return false;

                    return string.Equals(x.Key, y.Key) && string.Equals(x.Version, y.Version) && x.Language == y.Language;
                }

                public int GetHashCode(CompiledEntry obj)
                {
                    return $"{obj.Key}_{obj.Language}_{obj.Version}".GetHashCode();
                }
            }
        }
    }
}
