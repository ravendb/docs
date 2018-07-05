using System.Diagnostics;
using System.Linq;

namespace Raven.Documentation.Parser
{
	using System.Collections.Generic;
	using System.IO;

	using Raven.Documentation.Parser.Data;
	using Raven.Documentation.Parser.Helpers;

	internal class DocumentationDirectoryCompiler
	{
		private readonly DocumentCompiler _documentCompiler;

		private readonly ParserOptions _options;


		public DocumentationDirectoryCompiler(DocumentCompiler documentCompiler, ParserOptions options)
		{
			_documentCompiler = documentCompiler;
			_options = options;
		}

		public IEnumerable<DocumentationPage> Compile(TableOfContents tableOfContents)
		{
		    foreach (var tableOfContentsItem in tableOfContents.Items)
		    {
		        var tocItemCompilationResult = CompileTableOfContentsItem(tableOfContentsItem, tableOfContents.Version);

                foreach (var documentationPage in tocItemCompilationResult)
		        {
		            yield return documentationPage;
		        }
		    }
		}

        private DocumentationPage CompileDocumentationPage(FolderItem page, string directory,
            string documentationVersion, List<DocumentationMapping> mappings,
            string sourceDocumentationVersion = null)
        {
            var path = Path.Combine(directory,
                page.Name + FileExtensionHelper.GetLanguageFileExtension(page.Language) +
                Constants.MarkdownFileExtension);

            var fileInfo = new FileInfo(path);

            if (fileInfo.Exists == false)
                throw new FileNotFoundException($"Documentation file '{path}' not found.");

            var compilationParams = new DocumentationCompilation.Parameters
            {
                File = fileInfo,
                Page = page,
                DocumentationVersion = documentationVersion,
                SourceDocumentationVersion = sourceDocumentationVersion ?? documentationVersion,
                Mappings = mappings
            };

            return _documentCompiler.Compile(compilationParams);
        }

        public IEnumerable<DocumentationPage> CompileIndexFiles(DirectoryInfo directoryInfo)
        {
            var documentationVersion = directoryInfo.Name;

            var pagesDirectory = _options.GetPathToDocumentationPagesDirectory(documentationVersion);
            Debug.Assert(Directory.Exists(pagesDirectory));

            return CompileIndexFileInDirectory(pagesDirectory, documentationVersion);
        }

        private IEnumerable<DocumentationPage> CompileIndexFileInDirectory(string directory, string documentationVersion, List<DocumentationMapping> mappings = null)
        {
            var docsFilePath = Path.Combine(directory, Constants.DocumentationFileName);
            if (File.Exists(docsFilePath) == false)
                yield break;

            foreach (var item in DocumentationFileHelper.ParseFile(docsFilePath))
            {
                if (item.IsFolder == false)
                    continue;

                foreach (var page in CompileIndexFileInDirectory(Path.Combine(directory, item.Name), documentationVersion, item.Mappings))
                {
                    yield return page;
                }
            }

            var indexFilePath = Path.Combine(directory, "index" + Constants.MarkdownFileExtension);
            if (File.Exists(indexFilePath))
                yield return CompileIndexFile(directory, documentationVersion, mappings);
        }

        public DocumentationPage CompileIndexFile(string directory, string version, List<DocumentationMapping> mappings = null)
	    {
            var indexItem = new FolderItem(isFolder: false)
            {
                Description = string.Empty,
                Language = Language.All,
                Name = "index"
            };

            return CompileDocumentationPage(indexItem, directory, version, mappings ?? new List<DocumentationMapping>());
        }

	    private IEnumerable<DocumentationPage> CompileTableOfContentsItem(TableOfContents.TableOfContentsItem tocItem, string version)
	    {
	        if (tocItem.IsFolder)
	        {
	            foreach (var innerTocItem in tocItem.Items)
	            {
	                foreach (var innerTocItemCompiled in CompileTableOfContentsItem(innerTocItem, version))
	                {
	                    yield return innerTocItemCompiled;
	                }
	            }

	            yield break;
	        }

	        var directory = tocItem.GetSourceBaseDirectory(_options);
	        var pagesToCompile = GetPagesForTocItem(tocItem, directory);

	        foreach (var pageToCompile in pagesToCompile)
	        {
	            yield return CompileDocumentationPage(pageToCompile, directory, version, pageToCompile.Mappings, tocItem.SourceVersion);
	        }
	    }

	    private IEnumerable<FolderItem> GetPagesForTocItem(TableOfContents.TableOfContentsItem tocItem, string directory)
	    {
            var docsFilePath = Path.Combine(directory, Constants.DocumentationFileName);
            if (File.Exists(docsFilePath) == false)
                yield break;

	        var keySuffix = tocItem.GetKeySuffix();

            var matchingFolderItems = DocumentationFileHelper.ParseFile(docsFilePath)
                .Where(x => x.Name == keySuffix)
                .SelectMany(x => GetPages(directory, x));

	        foreach (var folderItem in matchingFolderItems)
	        {
	            yield return folderItem;
	        }
	    }

        internal static IEnumerable<FolderItem> GetPages(string directory, FolderItem item)
        {
            var path = Path.Combine(directory, item.Name + Constants.MarkdownFileExtension);
            if (File.Exists(path))
            {
                yield return item;
                yield break;
            }

            var languageFileExtensions = FileExtensionHelper.GetLanguageFileExtensions();

            foreach (var language in languageFileExtensions.Keys)
            {
                var extension = languageFileExtensions[language];
                var name = item.Name + extension;
                path = Path.Combine(directory, name + Constants.MarkdownFileExtension);
                if (File.Exists(path))
                {
                    yield return new FolderItem(item)
                    {
                        Language = language
                    };
                }
            }
        }
    }
}
