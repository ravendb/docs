using System;
using System.Collections.Generic;
using System.IO;
using Raven.Documentation.Parser.Data;

namespace Raven.Documentation.Parser
{
    public class DocumentationCompilation
    {
        public class Parameters
        {
            public FileInfo File { get; set; }
            public FolderItem Page { get; set; }
            public string DocumentationVersion { get; set; }
            public List<DocumentationMapping> Mappings { get; set; }
        }

        public class Context
        {
            private readonly HashSet<CompiledEntry> _compiled = new HashSet<CompiledEntry>(new CompiledEntryEqualityComparer());

            public void RegisterCompilation(string key, Language language, string baseVersion, List<string> supportedVersions)
            {
                RegisterCompilation(key, language, baseVersion);

                if (supportedVersions == null)
                    return;

                foreach (var version in supportedVersions)
                {
                    if (string.Equals(version, baseVersion))
                        continue;

                    RegisterCompilation(key, language, version);
                }
            }

            private void RegisterCompilation(string key, Language language, string version)
            {
                var entry = new CompiledEntry
                {
                    Key = key,
                    Language = language,
                    Version = version
                };

                if (_compiled.Contains(entry))
                    throw new InvalidOperationException($"The document '{key}' has already been compiled for {language} {version}");

                _compiled.Add(entry);
            }

            private class CompiledEntry
            {
                public string Key { get; set; }

                public Language Language { get; set; }

                public string Version { get; set; }
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
