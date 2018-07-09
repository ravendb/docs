using System.IO;
using System.Linq;
using Raven.Documentation.Parser.Data;

namespace Raven.Documentation.Parser.Helpers
{
    internal static class TableOfContentsItemExtensions
    {
        public static string GetDocumentationPageSourcePath(this TableOfContents.TableOfContentsItem tableOfContentsItem, ParserOptions parserOptions)
        {
            var documentationPagesDirectory = parserOptions.GetPathToDocumentationPagesDirectory(tableOfContentsItem.SourceVersion);
            return Path.Combine(documentationPagesDirectory, tableOfContentsItem.Key);
        }

        public static string GetSourceBaseDirectory(this TableOfContents.TableOfContentsItem tableOfContentsItem, ParserOptions parserOptions)
        {
            var filePath = GetDocumentationPageSourcePath(tableOfContentsItem, parserOptions);
            return Directory.GetParent(filePath).FullName;
        }

        public static string GetKeySuffix(this TableOfContents.TableOfContentsItem tableOfContentsItem)
        {
            return tableOfContentsItem.Key.Split('/').Last();
        }
    }
}
