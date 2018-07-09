using System.Collections.Generic;
using System.IO;
using Raven.Documentation.Parser.Data;
using Raven.Documentation.Parser.Helpers;

namespace Raven.Documentation.Parser.Compilation.DocumentationDirectory
{
    internal class DocumentationDirectoryCompiler
	{
		private readonly DocumentCompiler _documentCompiler;

		protected ParserOptions Options { get; }

		public DocumentationDirectoryCompiler(DocumentCompiler documentCompiler, ParserOptions options)
		{
			_documentCompiler = documentCompiler;
			Options = options;
		}

        protected DocumentationPage CompileDocumentationPage(FolderItem page, string directory,
            string documentationVersion, List<DocumentationMapping> mappings,
            string sourceDocumentationVersion = null)
        {
            var path = Path.Combine(directory,
                page.Name + FileExtensionHelper.GetLanguageFileExtension(page.Language) +
                Constants.MarkdownFileExtension);

            var fileInfo = new FileInfo(path);

            if (fileInfo.Exists == false)
                throw new FileNotFoundException($"Documentation file '{path}' not found.");

            var compilationParams = new CompilationUtils.Parameters
            {
                File = fileInfo,
                Page = page,
                DocumentationVersion = documentationVersion,
                SourceDocumentationVersion = sourceDocumentationVersion ?? documentationVersion,
                Mappings = mappings
            };

            return _documentCompiler.Compile(compilationParams);
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
