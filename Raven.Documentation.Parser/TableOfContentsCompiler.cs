using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Raven.Documentation.Parser.Data;
using Raven.Documentation.Parser.Helpers;

namespace Raven.Documentation.Parser
{
    internal class TableOfContentsCompiler
    {
        private readonly ParserOptions _options;

        public TableOfContentsCompiler(ParserOptions options)
        {
            _options = options;
        }

        public IEnumerable<TableOfContents> GenerateTableOfContents(DirectoryInfo directoryInfo)
        {
            var directoryName = directoryInfo.Name;
            var documentationVersion = directoryName;
            var directory = _options.GetPathToDocumentationPagesDirectory(documentationVersion);

            Debug.Assert(Directory.Exists(_options.GetPathToDocumentationPagesDirectory(documentationVersion)));

            var docsFilePath = Path.Combine(directory, Constants.DocumentationFileName);
            if (File.Exists(docsFilePath) == false)
                yield break;

            foreach (var item in DocumentationFileHelper.ParseFile(docsFilePath))
            {
                if (!item.IsFolder)
                    continue;

                var tableOfContents = new TableOfContents
                {
                    Version = documentationVersion,
                    Category = CategoryHelper.ExtractCategoryFromPath(item.Name),
                    Items = GenerateTableOfContentItems(Path.Combine(directory, item.Name), item.Name).ToList()
                };

                yield return tableOfContents;
            }
        }

        private static IEnumerable<TableOfContents.TableOfContentsItem> GenerateTableOfContentItems(string directory, string keyPrefix)
        {
            var docsFilePath = Path.Combine(directory, Constants.DocumentationFileName);
            if (File.Exists(docsFilePath) == false)
                yield break;

            foreach (var item in DocumentationFileHelper.ParseFile(docsFilePath))
            {
                var tableOfContentsItem = new TableOfContents.TableOfContentsItem
                {
                    Key = keyPrefix + "/" + item.Name,
                    Title = item.Description,
                    IsFolder = item.IsFolder
                };

                if (tableOfContentsItem.IsFolder)
                    tableOfContentsItem.Items = GenerateTableOfContentItems(Path.Combine(directory, item.Name), tableOfContentsItem.Key).ToList();
                else
                    tableOfContentsItem.Languages = GetLanguagesForTableOfContentsItem(directory, item.Name).ToList();

                yield return tableOfContentsItem;
            }
        }

        private static IEnumerable<Language> GetLanguagesForTableOfContentsItem(string directory, string tableOfContentsItemName)
        {
            var filePath = Path.Combine(directory, tableOfContentsItemName);

            var allFilePath = filePath + Constants.MarkdownFileExtension;
            if (File.Exists(allFilePath))
            {
                yield return Language.All;
                yield break;
            }

            foreach (var languageFileExtension in FileExtensionHelper.GetLanguageFileExtensions())
            {
                var languageFilePath = filePath + languageFileExtension.Value + Constants.MarkdownFileExtension;
                if (File.Exists(languageFilePath))
                    yield return languageFileExtension.Key;
            }
        }
    }
}
