using System.IO;
using System.Linq;
using Raven.Documentation.Parser.Data;

namespace Raven.Documentation.Parser.Helpers
{
    internal static class TableOfContentsItemExtensions
    {
        public static string GetSourceBaseDirectory(this TableOfContents.TableOfContentsItem tableOfContentsItem, ParserOptions parserOptions)
        {
            var version = tableOfContentsItem.SourceVersion;
            return GetDirectoryPath(tableOfContentsItem, version, parserOptions);
        }

        public static string GetDirectoryPath(this TableOfContents.TableOfContentsItem tableOfContentsItem, string version, ParserOptions parserOptions)
        {
            var documentationPagesDirectory = parserOptions.GetPathToDocumentationPagesDirectory(version);
            var filePath = Path.Combine(documentationPagesDirectory, tableOfContentsItem.Key);
            return Directory.GetParent(filePath).FullName;
        }

        public static string GetKeySuffix(this TableOfContents.TableOfContentsItem tableOfContentsItem)
        {
            return tableOfContentsItem.Key.Split('/').Last();
        }
    }
}
