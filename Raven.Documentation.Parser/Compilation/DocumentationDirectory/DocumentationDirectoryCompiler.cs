using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            var path = FileExtensionHelper.GetMarkdownFilePath(directory, page);

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
            var itemsForLanguages = FileExtensionHelper.GetItemsForLanguages(directory, item);
            return itemsForLanguages.Select(x => x.Item);
        }
    }
}
