using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Raven.Documentation.Parser.Data;
using Raven.Documentation.Parser.Helpers;

namespace Raven.Documentation.Parser.Compilation.ToC
{
    internal class TableOfContentsCompiler
    {
        private readonly ParserOptions _options;
        private readonly VersionsContainer _versions;
        private readonly TocConverter _converter = new TocConverter();

        public TableOfContentsCompiler(ParserOptions options)
        {
            _options = options;
            _versions = new VersionsContainer(options.PathToDocumentationDirectory);
        }

        public TocFolderCompilationResult Compile(DirectoryInfo directoryInfo, CompilationUtils.Context context)
        {
            var documentationVersion = directoryInfo.Name;
            var compiledTocs = GenerateTableOfContents(documentationVersion, context).ToList();

            return new TocFolderCompilationResult
            {
                CompiledTocs = compiledTocs,
                SourceDirectoryVersion = documentationVersion
            };
        }

        private IEnumerable<TableOfContents> GenerateTableOfContents(string documentationVersion, CompilationUtils.Context context)
        {
            var directory = _options.GetPathToDocumentationPagesDirectory(documentationVersion);

            Debug.Assert(Directory.Exists(directory));

            var docsFilePath = Path.Combine(directory, Constants.DocumentationFileName);
            if (File.Exists(docsFilePath) == false)
                yield break;

            foreach (var item in DocumentationFileHelper.ParseFile(docsFilePath))
            {
                if (!item.IsFolder)
                    continue;

                var category = CategoryHelper.ExtractCategoryFromPath(item.Name);

                var compilationResults = GenerateTableOfContentItems(Path.Combine(directory, item.Name), item.Name, documentationVersion, context).ToList();

                yield return _converter.ConvertToTableOfContents(compilationResults, documentationVersion, category);

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

        private IEnumerable<CompilationResult> GenerateTableOfContentItems(string directory, string keyPrefix,
            string documentationVersion, CompilationUtils.Context context)
        {
            var docsFilePath = Path.Combine(directory, Constants.DocumentationFileName);
            if (File.Exists(docsFilePath) == false)
                yield break;

            foreach (var item in DocumentationFileHelper.ParseFile(docsFilePath))
            {
                Validate(item, docsFilePath);

                var tableOfContentsItem = new CompilationResult
                {
                    Key = keyPrefix + "/" + item.Name,
                    Title = item.Description,
                    IsFolder = item.IsFolder
                };

                if (item.IsFolder)
                {
                    var compilationResults = GenerateTableOfContentItems(Path.Combine(directory, item.Name),
                        tableOfContentsItem.Key, documentationVersion, context).ToList();

                    tableOfContentsItem.Items = compilationResults;
                    var supportedVersions = compilationResults.SelectMany(x => x.SupportedVersions);
                    tableOfContentsItem.AddSupportedVersions(supportedVersions);
                }
                else
                {
                    tableOfContentsItem.Languages = GetLanguagesForTableOfContentsItem(directory, item.Name).ToList();

                    if (MarkdownFileExists(directory, item))
                    {
                        var supportedVersions =
                            _versions.GetMinorVersionsInRange(documentationVersion, item.LastSupportedVersion);

                        tableOfContentsItem.AddSupportedVersions(supportedVersions);
                        tableOfContentsItem.SourceVersion = documentationVersion;
                    }

                    context.RegisterCompilation(new CompilationUtils.Context.RegistrationInput
                    {
                        Key = tableOfContentsItem.Key,
                        Language = item.Language,
                        DocumentationVersion = documentationVersion,
                        LastSupportedVersion = item.LastSupportedVersion,
                        SupportedVersions = tableOfContentsItem.SupportedVersions?.ToList()
                    });
                }

                yield return tableOfContentsItem;
            }
        }

        private bool MarkdownFileExists(string directory, FolderItem item)
        {
            var itemsForLanguages = FileExtensionHelper.GetItemsForLanguages(directory, item);
            return itemsForLanguages.Any();
        }

        private void Validate(FolderItem item, string docsFilePath)
        {
            if (string.IsNullOrWhiteSpace(item.Description))
                throw new InvalidOperationException($"'Name' parameter cannot be empty. Entry: {item.Name}. Path: {docsFilePath}");
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

                var tocItem = _converter.ConvertToTableOfContentsItem(compilationResult);
                tocItem.Items = GetAdditionalSupportedTableOfContentItems(supportedVersion, compilationResult.Items).ToList();

                yield return tocItem;
            }
        }

        internal class CompilationResult
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

        internal class TocFolderCompilationResult
        {
            public List<TableOfContents> CompiledTocs { get; set; }

            public string SourceDirectoryVersion { get; set; }
        }
    }
}
