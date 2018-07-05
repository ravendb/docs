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

        public IEnumerable<TableOfContents> GenerateTableOfContents(DirectoryInfo directoryInfo, DocumentationCompilation.Context context)
        {
            var documentationVersion = directoryInfo.Name;
            var directory = _options.GetPathToDocumentationPagesDirectory(documentationVersion);

            Debug.Assert(Directory.Exists(_options.GetPathToDocumentationPagesDirectory(documentationVersion)));

            var docsFilePath = Path.Combine(directory, Constants.DocumentationFileName);
            if (File.Exists(docsFilePath) == false)
                yield break;

            foreach (var item in DocumentationFileHelper.ParseFile(docsFilePath))
            {
                if (!item.IsFolder)
                    continue;

                var compilationResults = GenerateTableOfContentItems(Path.Combine(directory, item.Name), item.Name, documentationVersion, context).ToList();

                var tocItems = ConvertToTableOfContentItems(compilationResults).ToList();

                var category = CategoryHelper.ExtractCategoryFromPath(item.Name);

                var tableOfContents = new TableOfContents
                {
                    Version = documentationVersion,
                    Category = category,
                    Items = tocItems
                };

                yield return tableOfContents;

                var additionalSupportedVersions = GetAdditionalSupportedVersions(documentationVersion, compilationResults);

                foreach (var supportedVersion in additionalSupportedVersions)
                {
                    yield return GetAdditionalSupportedTableOfContent(supportedVersion, category, compilationResults);
                }
            }
        }

        private IEnumerable<string> GetAdditionalSupportedVersions(string baseVersion, IEnumerable<CompilationResult> compilationResults)
        {
            var supportedVersions = compilationResults.SelectMany(x => x.SupportedVersions).Distinct();
            return supportedVersions.Where(x => x != baseVersion);
        }

        private IEnumerable<CompilationResult> GenerateTableOfContentItems(string directory, string keyPrefix, string documentationVersion, DocumentationCompilation.Context context)
        {
            var docsFilePath = Path.Combine(directory, Constants.DocumentationFileName);
            if (File.Exists(docsFilePath) == false)
                yield break;

            foreach (var item in DocumentationFileHelper.ParseFile(docsFilePath))
            {
                var tableOfContentsItem = new CompilationResult
                {
                    Key = keyPrefix + "/" + item.Name,
                    Title = item.Description,
                    IsFolder = item.IsFolder
                };

                if (tableOfContentsItem.IsFolder)
                {
                    var compilationResults = GenerateTableOfContentItems(Path.Combine(directory, item.Name), tableOfContentsItem.Key, documentationVersion, context).ToList();
                    tableOfContentsItem.Items = compilationResults;
                    var supportedVersions = compilationResults.SelectMany(x => x.SupportedVersions);
                    tableOfContentsItem.AddSupportedVersions(supportedVersions);
                }
                else
                {
                    tableOfContentsItem.Languages = GetLanguagesForTableOfContentsItem(directory, item.Name).ToList();
                    tableOfContentsItem.AddSupportedVersions(item.SupportedVersions);
                    tableOfContentsItem.SourceVersion = documentationVersion;
                    context.RegisterCompilation(tableOfContentsItem.Key, item.Language, documentationVersion, item.SupportedVersions);
                }

                yield return tableOfContentsItem;
            }
        }

        private IEnumerable<Language> GetLanguagesForTableOfContentsItem(string directory, string tableOfContentsItemName)
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

        private IEnumerable<TableOfContents.TableOfContentsItem> ConvertToTableOfContentItems(IEnumerable<CompilationResult> compilationResults)
        {
            foreach (var compilationResult in compilationResults)
            {
                var tocItem = ConvertToTableOfContentsItem(compilationResult);
                tocItem.Items = ConvertToTableOfContentItems(compilationResult.Items).ToList();

                yield return tocItem;
            }
        }

        private TableOfContents.TableOfContentsItem ConvertToTableOfContentsItem(CompilationResult compilationResult)
        {
            var tocItem = new TableOfContents.TableOfContentsItem
            {
                Key = compilationResult.Key,
                Title = compilationResult.Title,
                SourceVersion = compilationResult.SourceVersion,
                IsFolder = compilationResult.IsFolder,
                Languages = compilationResult.Languages
            };
            return tocItem;
        }

        private TableOfContents GetAdditionalSupportedTableOfContent(string supportedVersion, Category category, List<CompilationResult> compilationResults)
        {
            var tableOfContents = new TableOfContents
            {
                Version = supportedVersion,
                Category = category
            };

            tableOfContents.Items = GetAdditionalSupportedTableOfContentItems(supportedVersion, compilationResults).ToList();

            return tableOfContents;
        }

        private IEnumerable<TableOfContents.TableOfContentsItem> GetAdditionalSupportedTableOfContentItems(string supportedVersion, List<CompilationResult> compilationResults)
        {
            foreach (var compilationResult in compilationResults)
            {
                var hasSupportedVersion = compilationResult.SupportedVersions.Contains(supportedVersion);

                if (hasSupportedVersion == false)
                    continue;

                var tocItem = ConvertToTableOfContentsItem(compilationResult);
                tocItem.Items = GetAdditionalSupportedTableOfContentItems(supportedVersion, compilationResult.Items).ToList();

                yield return tocItem;
            }
        }

        private class CompilationResult
        {
            private readonly HashSet<string> _supportedVersions = new HashSet<string>();

            public CompilationResult()
            {
                Items = new List<CompilationResult>();
                Languages = new List<Language>();
            }

            public string Key { get; set; }

            public string Title { get; set; }

            public string SourceVersion { get; set; }

            public bool IsFolder { get; set; }

            public List<CompilationResult> Items { get; set; }

            public List<Language> Languages { get; set; }

            public IEnumerable<string> SupportedVersions => _supportedVersions;

            public void AddSupportedVersions(IEnumerable<string> supportedVersions)
            {
                _supportedVersions.UnionWith(supportedVersions);
            }
        }
    }
}
